﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titanium.Core.Tokens;

namespace Titanium.Test
{
	[TestClass]
	public class TokenizeTests
	{
		[TestMethod]
		public void EmptyStringTest()
		{
			var tokens = Tokenizer.Tokenize(string.Empty);
			Assert.AreEqual(0, tokens.Count);
		}

		[TestMethod]
		public void SingleStringTest()
		{
			var tokens = Tokenizer.Tokenize("1");
			Assert.AreEqual(1, tokens.Count);
			Assert.AreEqual("1", tokens[0].Value);
		}

		[TestMethod]
		public void DoubleStringTest()
		{
			var tokens = Tokenizer.Tokenize("1a");
			Assert.AreEqual(2, tokens.Count);
			Assert.AreEqual("1", tokens[0].Value);
			Assert.AreEqual("a", tokens[1].Value);
		}

		[TestMethod]
		public void QuadrupleStringTest()
		{
			var tokens = Tokenizer.Tokenize("(1a)");
			Assert.AreEqual(4, tokens.Count);
			Assert.AreEqual("(", tokens[0].Value);
			Assert.AreEqual("1", tokens[1].Value);
			Assert.AreEqual("a", tokens[2].Value);
			Assert.AreEqual(")", tokens[3].Value);
		}

		[TestMethod]
		public void LongTokenTest()
		{
			var tokens = Tokenizer.Tokenize("hello");
			Assert.AreEqual(1, tokens.Count);
			Assert.AreEqual("hello", tokens[0].Value);
		}

		[TestMethod]
		public void SpaceTest()
		{
			var tokens = Tokenizer.Tokenize("(hello + 2)");
			Assert.AreEqual(5, tokens.Count);
			Assert.AreEqual("(", tokens[0].Value);
			Assert.AreEqual("hello", tokens[1].Value);
			Assert.AreEqual("+", tokens[2].Value);
			Assert.AreEqual("2", tokens[3].Value);
			Assert.AreEqual(")", tokens[4].Value);
		}

		[TestMethod]
		public void ConsolidateNegativeTest()
		{
			ConsolidateSingle("⁻2");
		}

		[TestMethod]
		public void ConsolidateDecimalTest()
		{
			ConsolidateSingle("1.5");
		}

		[TestMethod]
		public void ConsolidateTwoValuesTest()
		{
			var tokens = Tokenizer.Tokenize("2.0+⁻2");
			tokens = Tokenizer.ConsolidateNumerics(tokens);
			Assert.AreEqual(3, tokens.Count);
			Assert.AreEqual("2.0", tokens[0].Value);
			Assert.AreEqual(TokenType.Number, tokens[0].Type);
			Assert.AreEqual("+", tokens[1].Value);
			Assert.AreEqual(TokenType.Plus, tokens[1].Type);
			Assert.AreEqual("⁻2", tokens[2].Value);
			Assert.AreEqual(TokenType.Number, tokens[2].Type);
		}

		private void ConsolidateSingle(string value)
		{
			var tokens = Tokenizer.Tokenize(value);
			tokens = Tokenizer.ConsolidateNumerics(tokens);
			Assert.AreEqual(1, tokens.Count);
			Assert.AreEqual(value, tokens[0].Value);
			Assert.AreEqual(TokenType.Number, tokens[0].Type);
		}
	}
}
