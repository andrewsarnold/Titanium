# Titanium

Titanium is a .NET mathematics parser and evaluator whose behavior is modeled off of the TI-89 calculator's engine.

Thanks to the string-input-to-string-output structure of math, Titanium is easy to test and I try to test every function I implement in as many ways as possible. New requirements are defined with unit tests. There are currently a few broken tests indicating functionality that hasn't been implemented yet.

## Motivation

An exploration of TDD and something to exercise the math parts of my brain before they disappear forever.

## Usage

Currently there are three ways to use Titanium:

1. Directly invoking it from a C# library, with the syntax:

 ```c#
 var evaluator = new Titanium.Core.Evaluator;
 var result = evaluator.Evaluate("input");
 ```
 
2. Using the Windows console application (best option for casual use)
3. Using the Windows desktop application

## Current features

* Basic arithmetic
* List operations
* Basic trigonometry
* Logarithms

## Quirks

Titanium uses the superscript-dash `⁻` to indicate negation. Because this symbol isn't easy to type it will also try to parse a hyphen `-` for both subtraction and negation. I haven't run into an issue with this so far.
