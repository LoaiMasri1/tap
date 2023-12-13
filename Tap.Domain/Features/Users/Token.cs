using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Features.Users
{
    public class Token : ValueObject
    {
        private Token() { }

        public Token(string value, DateTime expiredAt)
        {
            Ensure.NotEmpty(value, "The value is required.", nameof(value));
            Ensure.NotDefault(expiredAt, "The expired at is required.", nameof(expiredAt));
            Ensure.NotPast(expiredAt, "The expired at must be in the future.", nameof(expiredAt));

            Value = value;
            ExpiredAt = expiredAt;
        }

        public string Value { get; private set; }
        public DateTime ExpiredAt { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return ExpiredAt;
        }
    }
}
