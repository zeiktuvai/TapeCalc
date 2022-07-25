using System.Collections.Generic;
using System.Collections.ObjectModel;
using TapeCalc.Models;

namespace TapeCalc.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //public MainWindowViewModel(IEnumerable<CalcultorLineItemModel> lineItems)
        //{
        //    LineItems = new ObservableCollection<CalcultorLineItemModel>(lineItems);
        //}
        //public ObservableCollection<CalcultorLineItemModel> LineItems { get; set; }
        public string Greeting => "yo";
    }
}
