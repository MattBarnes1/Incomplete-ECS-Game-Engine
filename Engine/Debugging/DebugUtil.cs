using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Debugging
{
    public static class DebugUtil
    {
        [TestReminder("DebugUtil", "Classes with wierd or unusual properties or types")]
        private static String DumpInformationAboutObject<T>(int depth, int maxDepth, T myObject)
        {
            string Tabs = CreateTabOffset(depth);
            if (depth >= maxDepth)
            {
                return "Maximum depth reached!";
            }
            StringBuilder Data = new StringBuilder(1024);
            if (depth != 0)
            {
                Data.AppendLine(Tabs + "{");
            }
            Dictionary<Type, List<String>> myOutput = new Dictionary<Type, List<string>>();
            List<Tuple<String, List<String>>> myOutputSortedVariables = new List<Tuple<String, List<String>>>();
            var ObjectType = myObject.GetType();

            var innerTabs = Tabs + "\t";
            Data.AppendLine(Tabs + "Class: " + ObjectType.Name);
            if (ObjectType.IsArray)
            {
                Data.AppendLine(HandleArray(depth, maxDepth, myObject));
            }
            else if (myObject is IList)
            {
                Data.AppendLine(HandleListType(depth, maxDepth, myObject));
            }
            else if (myObject is IDictionary)
            {
                Data.AppendLine(HandleDictionaryType(depth, maxDepth, myObject));
            }
            else
            {
                var AB = ObjectType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                foreach (var Variable in AB)
                {
                    String ConvertedString = "";
                    var Value = Variable.GetValue(myObject);
                    var VariableType = Variable.FieldType;
                    ConvertedString = ProcessValue(depth, maxDepth, Value, VariableType);

                    if (myOutput.ContainsKey(Variable.FieldType))
                    {
                        myOutput[Variable.FieldType].Add(innerTabs + "[Value] " + Variable.Name + " : " + ConvertedString);
                    }
                    else
                    {
                        List<String> myValue = new List<string>()
                        {
                            innerTabs + "[Value] " + Variable.Name + " : " + ConvertedString
                        };
                        myOutput[Variable.FieldType] = myValue;
                        myOutputSortedVariables.Add(new Tuple<String, List<String>>(Variable.FieldType.ToString(), myValue));
                    }
                }

                foreach (var Properties in ObjectType.GetProperties())
                {
                    if (Properties.GetMethod.GetParameters().Count() == 0)
                    {
                        String ConvertedString = "";
                        var Value = Properties.GetValue(myObject);
                        var VariableType = Properties.PropertyType;
                        ConvertedString = ProcessValue(depth, maxDepth, Value, VariableType);
                        if (myOutput.ContainsKey(Properties.PropertyType))
                        {
                            myOutput[Properties.PropertyType].Add(innerTabs + "[Property] " + Properties.Name + " : " + ConvertedString);
                        }
                        else
                        {
                            List<String> myValue = new List<string>()
                            {
                                innerTabs + "[Property] " + Properties.Name + " : " + ConvertedString
                            };
                            myOutput[Properties.PropertyType] = myValue;
                            myOutputSortedVariables.Add(new Tuple<String, List<String>>(Properties.PropertyType.ToString(), myValue));
                        }
                    }
                }

                myOutputSortedVariables.Sort(delegate (Tuple<String, List<String>> a, Tuple<String, List<String>> b)
                {
                    return b.Item1.CompareTo(a.Item1); //makes greatest first
                });

                foreach (var TupleVal in myOutputSortedVariables)
                {
                    foreach (var StringOutput in TupleVal.Item2)
                    {
                        Data.AppendLine(StringOutput);
                    }
                }
            }
            if (depth != 0)
            {
                Data.AppendLine(Tabs + "}");
            }



            return Data.ToString();
        }

        private static string CreateTabOffset(int depth)
        {
            String Tabs = "";
            for (int i = 0; i < depth; i++)
            {
                Tabs += "\t";
            }

            return Tabs;
        }

        private static string ProcessValue(int depth, int maxDepth, object Value, Type VariableType)
        {
            string ConvertedString = "";
            if (Value == null)
            {
                ConvertedString = "null";
            }
            else if (VariableType == typeof(String))
            {
                ConvertedString = "\"" + (String)Value + "\"";
            }
            else if (Value is IList)
            {
                ConvertedString = HandleListType(depth + 1, maxDepth, Value);
            }
            else if (Value is IDictionary)
            {
                ConvertedString = HandleDictionaryType(depth + 1, maxDepth, Value);
            }
            else if (VariableType.IsArray)
            {
                ConvertedString = HandleArray(depth + 1, maxDepth, Value);
            }
            else if (VariableType.IsClass)
            {
                ConvertedString = DumpInformationAboutObject(depth + 1, maxDepth, Value);
            }
            else
            {
                ConvertedString = Value.ToString();
            }
            return ConvertedString;
        }

        private static string HandleDictionaryType(int depth, int maxDepth, object Value)
        {
            IDictionary myDictionary = (IDictionary)Value;
            int KeyNumber = 0;
            String myReturn = "{";
            var Item = CreateTabOffset(depth + 1);
            if (myDictionary.Values.Count != 0)
            {
                myReturn = "\n" + Item + myReturn + "\n";
                foreach (var A in myDictionary.Values)
                {
                    myReturn += Item + "Key #" + KeyNumber++;
                    if (A.GetType().IsValueType)
                    {
                        myReturn += A.ToString() + "\n";
                    }
                    else
                    {
                        myReturn += DumpInformationAboutObject(depth + 1, maxDepth, A) + "\n";
                    }

                }
                myReturn += Item + " }";
            } 
            else
            {
                myReturn += "}";
            }
            return myReturn;
        }

        private static string HandleListType(int depth, int maxDepth, object Object)
        {
            IList myObjects = (IList)Object;
            String myReturn = "{";
            var Item = CreateTabOffset(depth + 1);
            if(myObjects.Count != 0)
            {
                myReturn = "\n" + Item + myReturn + "\n";
                foreach (Object A in myObjects)
                {
                    if (A.GetType().IsValueType)
                    {
                        myReturn += Item + A.ToString() + "\n";
                    }
                    else
                    {
                        myReturn += DumpInformationAboutObject(depth + 1, maxDepth, A) + "\n";
                    }
                }
                myReturn += Item + " }";

            } 
            else
            {
                myReturn += "}";

            }

            return myReturn;
        }

        private static string HandleArray(int depth, int maxDepth, Object myObject)
        {
            Array aR = (Array)myObject;
            String myReturn = "{";
            var Item = CreateTabOffset(depth + 1);
            if(aR.Length != 0)
            {
                myReturn = "\n" + Item + myReturn + "\n";
                foreach (var myInput in aR)
                {
                    if (myInput != null)
                    {
                        if (myInput.GetType().IsValueType)
                        {
                            myReturn += Item + "\"" + myInput.ToString() + "\"" + "\n";
                        }
                        else
                        {
                            myReturn += DumpInformationAboutObject(depth + 1, maxDepth, myInput) + "\n";
                        }
                    }
                }

                myReturn += " }";
            } 
            else
            {
                myReturn += "}";
            }
            return myReturn;
        }

        public static String DumpInformationAboutObject<T>(int MaxDepth, T myObject)
        {
            
            return DumpInformationAboutObject<T>(0, MaxDepth, myObject);
        }


    }
}
