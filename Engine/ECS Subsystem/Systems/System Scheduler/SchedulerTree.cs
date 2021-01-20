
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Engine.Systems;

namespace Engine.ECS.Scheduler.SystemSchedulerTree
{
    public class SchedulerTree
    {
        List<List<Engine.Systems.System>> mySystems = new List<List<Engine.Systems.System>>();

        public readonly int coreCount = 0; //This is the number of items


        public int RowHeight {
            get
            {
                return mySystems.Count;
            }
        }

        public SchedulerTree()
        {

            mySystems.Add(new List<Engine.Systems.System>());
        }

        private void AddSystemAtLevel(Engine.Systems.System mySystem, int Level)
        {
            if(mySystems.Count <= Level)
            {
                mySystems.Add(new List<Engine.Systems.System>() { mySystem });
            }
            else
            {

                int Output;
                switch (CheckForConflict(mySystem, mySystems[Level], out Output))
                {
                    case ConflictType.None:
                        return;
                        break;
                    case ConflictType.IncomingSystemReaderWriter:
                        AddSystemAtLevel(mySystem, Level + 1);
                        break;
                    case ConflictType.IncomingSystemWriterReader:
                        List<Engine.Systems.System> MyRemovedSystems = new List<Engine.Systems.System>();
                        MyRemovedSystems.Add(mySystems[Level][Output]);
                        mySystems[Level].Remove(mySystems[Level][Output]);
                        while (CheckForConflict(mySystem, mySystems[Level], out Output) != ConflictType.None)
                        {
                            MyRemovedSystems.Add(mySystems[Level][Output]);
                            mySystems[Level].Remove(mySystems[Level][Output]);
                        }
                        foreach (Engine.Systems.System A in MyRemovedSystems)
                        {
                            AddSystemAtLevel(A, Level + 1);
                        }
                        break;
                    case ConflictType.IncomingSystemLevelLessThan:
                        mySystems.Insert(Level, new List<Engine.Systems.System>());
                        mySystems[Level].Add(mySystem);
                        break;
                    case ConflictType.IncomingSystemGreaterThan:
                        AddSystemAtLevel(mySystem, Level + 1);
                        break;
                    case ConflictType.IncomingSystemDoubleWriter:
                        Engine.Systems.System DoubleWriter = SelectHighestPriority(mySystems[Level][Output], mySystem);
                        if (mySystems[Level][Output] == DoubleWriter)
                        {
                            mySystems[Level].RemoveAt(Output);
                            mySystems[Level].Add(mySystem);
                        }
                        AddSystemAtLevel(DoubleWriter, Level + 1);
                        break;
                    default:
                        break;
                }
            }
        }

        private Engine.Systems.System SelectHighestPriority(Engine.Systems.System system, Engine.Systems.System mySystem)
        {
            if (system.ExecutionOrder > mySystem.ExecutionOrder)
                return system;
            else if (system.ExecutionOrder < mySystem.ExecutionOrder)
                return mySystem;
            else
            {
                throw new Exception("System can't resolve Execution Priority.");
            }
        }

        internal Task RebuildThreadingSystem()
        {
            Task First = null;
            Task Last = null;
            for(int i = 0; i < mySystems.Count; i++)
            {
                if (First == null)
                {
                    List<Engine.Systems.System> mySystems = GetSystemRow(i);
                    if(mySystems.Count != 0)
                    {
                        First = new Task(delegate ()
                        {
                            foreach(Engine.Systems.System A in mySystems)
                            {

                            }  
                        });
                    }
                }
            }
            return First;
        }

        public void AddSystem(Engine.Systems.System mySystem)
        {
            AddSystemAtLevel(mySystem, 0);
        }

        enum ConflictType
        {
            None,
            IncomingSystemReaderWriter,
            IncomingSystemWriterReader,
            IncomingSystemLevelLessThan,
            IncomingSystemGreaterThan,
            IncomingSystemDoubleWriter,
        }

        private ConflictType CheckForConflict(Engine.Systems.System mySystem, List<Engine.Systems.System> ExistingSystems, out int ConflictPosition)
        {
            int FirstNull;
            if(ExistingSystems.Count() != 0)
            {
                for (int i = 0; i < ExistingSystems.Count; i++)
                {
                    if (ExistingSystems[i] != null)
                    {
                        var ThisReader = ExistingSystems[i].GetReaderEntityBits().GetAndBits();
                        var ThisWriter = ExistingSystems[i].GetWriterEntityBits().GetAndBits();
                        var NewSystemWriter = mySystem.GetWriterEntityBits().GetAndBits();
                        var NewSystemReader = mySystem.GetReaderEntityBits().GetAndBits(); //TODO: Test OR Bits
                        if(ThisWriter != null && NewSystemWriter != null && ThisWriter.AndTest(NewSystemWriter))
                        {
                            ConflictPosition = i;
                            return ConflictType.IncomingSystemDoubleWriter;
                        }
                        if ((ThisReader != null && NewSystemWriter != null && ThisReader.AndTest(NewSystemWriter)))
                        {
                            ConflictPosition = i;
                            return ConflictType.IncomingSystemWriterReader;
                        }
                        else if (ThisWriter != null && NewSystemReader != null && (ThisWriter.AndTest(NewSystemReader)))
                        {
                            ConflictPosition = i;
                            return ConflictType.IncomingSystemReaderWriter;
                        }
                        else if(ExistingSystems[i].ExecutionOrder > mySystem.ExecutionOrder)
                        {
                            ConflictPosition = i;
                            return ConflictType.IncomingSystemLevelLessThan;
                        }
                        else if (ExistingSystems[i].ExecutionOrder < mySystem.ExecutionOrder)
                        {
                            ConflictPosition = i;
                            return ConflictType.IncomingSystemGreaterThan;
                        }
                    }
                }
            }
            
            ExistingSystems.Add(mySystem);
            ConflictPosition = 0;
            return ConflictType.None;
        }
        public List<Engine.Systems.System> GetSystemRow(int v)
        {
            if(v >= mySystems.Count)
            {
                throw new Exception("Invalid System Row was requested from System Scheduler. System Row Too High!");
            }
            else if(v < 0)
            {
                throw new Exception("Invalid System Row was requested from System Scheduler. System Row Too Low!");
            }
            return new List<Engine.Systems.System>(mySystems[v]);
        }
    }
}
