using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class TestReminderAttribute : Attribute
{

    string ClassNeedingTests;
    string TestType;
    // This is a positional argument
    public TestReminderAttribute(string ClassNeedingTests, string TestType = "Unspecified")
    {
        this.TestType = TestType;
        this.ClassNeedingTests = ClassNeedingTests;
    }

    public void PrintTestInformational()
    {
        Debug.WriteLine("Test Cases needed for " + ClassNeedingTests + ": " + TestType);
    }
}
