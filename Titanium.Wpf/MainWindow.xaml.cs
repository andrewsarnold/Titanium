using System.Windows.Controls;

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
			MenuInsert.Items.Add(menu);
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
	}
}
