using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;

namespace JobSystem.TestHelpers
{
    /// <summary>
    /// Contains some little utility methods so that the error message can be verified. These should be used to verify that exceptions were thrown.
    /// The NUnit ExpectedException attribute can't be used because the domain validation can contain multiple results. In the case of a unit test though,
    /// you should only be testing one specific error, which is why we test on the first property in the collection.
    /// 
    /// You could still use the ExpectedException attribute, but you should really verify the message to make sure that the right error message is being produced.
    /// </summary>
    public static class DomainValidationExtensions
    {
        public static bool ResultContainsMessage(this DomainValidationException dex, string expectedMessage)
        {
            return dex.Result.Any(r => r.ErrorMessage == expectedMessage);
        }

        public static bool ResultPropertyContainsMessage(this DomainValidationException dex, string propertyName, string expectedMessage)
        {
            return dex.Result[0].MemberNames.ToArray()[0] == propertyName && dex.Result[0].ErrorMessage == expectedMessage;
        }
    }
}