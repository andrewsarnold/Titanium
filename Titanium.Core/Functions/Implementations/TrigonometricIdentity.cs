using Titanium.Core.Factors;

namespace Titanium.Core.Functions.Implementations
{
	internal class TrigonometricIdentity
	{
		public readonly string Function;
		public readonly Factor Input;
		public readonly Factor Output;

		public TrigonometricIdentity(string function, Factor input, Factor output)
		{
			Function = function;
			Input = input;
			Output = output;
		}
	}
}
