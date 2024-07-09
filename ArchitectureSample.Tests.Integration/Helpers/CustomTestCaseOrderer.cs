using Xunit.Abstractions;
using Xunit.Sdk;

namespace ArchitectureSample.Tests.Integration.Helpers;

public class CustomTestCaseOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase =>
        testCases.OrderBy(tc =>
        {
            var testOrderAttribute = tc.TestMethod.Method.GetCustomAttributes(typeof(TestOrderAttribute)).FirstOrDefault();
            if (testOrderAttribute is TestOrderAttribute orderAttribute)
                return orderAttribute.Priority;
            return int.MaxValue;
        });
}