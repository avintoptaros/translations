using dotMorten.Xamarin.Forms;
using System;
using TranslationNTT.ViewModels;
using Xamarin.Forms;

namespace TranslationNTT.Views
{
	public partial class MainPage : ContentPage
	{

        TranslatorViewModel _viewModel;

		public MainPage()
		{
			InitializeComponent();

            _viewModel = new TranslatorViewModel();
            BindingContext = _viewModel;

        }

        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            _viewModel.GetTranslations();
        }
    }
}
