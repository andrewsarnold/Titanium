﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Titanium.Core;
using Titanium.Core.Functions;
using Titanium.Wpf.Symbols;

namespace Titanium.Wpf
{
	public partial class MainWindow
	{
		private readonly Evaluator _evaluator;

		public MainWindow()
		{
			InitializeComponent();
			_evaluator = new Evaluator();
			FillInsertMenu();
			InputBox.Focus();
		}

		private void FillInsertMenu()
		{
			FillCommonMenu();
			FillFunctionMenu();
			FillSymbolMenu();
		}

		private void FillCommonMenu()
		{
			MenuCommon.Items.Add(SymbolMenuItem("⁻", "negative"));
		}

		private void FillFunctionMenu()
		{
			var menu = new MenuItem { Header = "Functions" };
			foreach (var function in FunctionRepository.AllNames.OrderBy(f => f))
			{
				menu.Items.Add(FunctionMenuItem(function));
			}
			MenuInsert.Items.Add(menu);
		}

		private MenuItem FunctionMenuItem(string name)
		{
			var menuItem = new MenuItem { Header = name };
			menuItem.Click += (sender, args) =>
			{
				InputBox.SelectedText = string.Format("{0}()", name);
				InputBox.CaretIndex += name.Length + 1;
				InputBox.Focus();
			};
			return menuItem;
		}

		private void FillSymbolMenu()
		{
			var menu = new MenuItem { Header = "Symbols" };
			menu.Items.Add(SymbolMenuItem("⁻", "negative"));
			menu.Items.Add(SymbolMenuItem("√", "square root"));
			menu.Items.Add(SymbolMenuItem("→", "assignment"));
			menu.Items.Add(GreekAlphabet());
			MenuInsert.Items.Add(menu);
		}

		private MenuItem GreekAlphabet()
		{
			var menu = new MenuItem { Header = "Greek" };
			foreach (var symbol in Greek.Symbols)
			{
				menu.Items.Add(SymbolMenuItem(symbol.Key, symbol.Value));
			}
			return menu;
		}

		private MenuItem SymbolMenuItem(string symbol, string name)
		{
			var menuItem = new MenuItem { Header = string.Format("{0} ({1})", symbol, name) };
			menuItem.Click += (sender, args) =>
			{
				InputBox.SelectedText = symbol;
				InputBox.CaretIndex += symbol.Length;
				InputBox.Focus();
			};
			return menuItem;
		}

		private void InputKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				AddString(InputBox.Text);

				string result;
				try
				{
					result = _evaluator.Evaluate(InputBox.Text);
				}
				catch (Exception ex)
				{
					result = ex.Message;
				}
				
				AddString(result, false);
				InputBox.Clear();
			}
		}

		private void AddString(string value, bool isLeftAligned = true)
		{
			History.Children.Add(new TextBlock
			{
				Text = value,
				HorizontalAlignment = isLeftAligned ? HorizontalAlignment.Left : HorizontalAlignment.Right,
				TextAlignment = isLeftAligned ? TextAlignment.Left : TextAlignment.Right,
				TextWrapping = TextWrapping.Wrap
			});
		}
	}
}
