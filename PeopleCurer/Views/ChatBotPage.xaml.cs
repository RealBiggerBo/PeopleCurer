using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

public partial class ChatBotPage : ContentPage
{
	public ChatBotPage()
	{
		InitializeComponent();

		((ChatBotPageViewModel)BindingContext).onResponseReceiveEvent += OnResponseReceive;
		((ChatBotPageViewModel)BindingContext).onMessageSendEvent += OnMessageSend;
	}

    private void OnMessageSend(object? obj, EventArgs e)
    {
		sendButton.IsEnabled = false;
		messageBox.Text = string.Empty;
    }

	private void OnResponseReceive(object? obj, EventArgs e)
	{
        sendButton.IsEnabled = true;
    }
}