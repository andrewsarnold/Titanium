using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Titanium.Wpf.Symbols;
using Expression = Titanium.Core.Expressions.Expression;

namespace Titanium.Wpf
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			FillInsertMenu();
			InputBox.Focus();
		}
		
		private void FillInsertMenu()
		{
			FillSymbolMenu();
		}

		private void FillSymbolMenu()
		{
			var menu = new MenuItem { Header = "Symbols" };
			menu.Items.Add(SymbolMenuItem("⁻", "negative"));
			menu.Items.Add(SymbolMenuItem("√", "square root"));
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
				History.Children.Add(new TextBlock{ Text = InputBox.Text, HorizontalAlignment = HorizontalAlignment.Left });

				string result;
				try
				{
					result = Expression.ParseExpression(InputBox.Text).Evaluate().ToString();
				}
				catch (Exception ex)
				{
					result = ex.Message;
				}
				
				History.Children.Add(new TextBlock { Text = result, HorizontalAlignment = HorizontalAlignment.Right });
				InputBox.Clear();
			}
		}
	}
}
