using Microsoft.Maui.Graphics.Text;

namespace StyleValuesAppliedTooEarly;

public partial class FancyControl : ContentView
{
    public static readonly BindableProperty PrimaryTextProperty =
        BindableProperty.Create(nameof(PrimaryText), typeof(string),
            typeof(FancyControl),
            default(string),
            propertyChanged:
            (bindable, oldValue, newValue) =>
                ((FancyControl)bindable).updatePrimaryLabelText());

    public string PrimaryText
    {
        set => SetValue(PrimaryTextProperty, value);
        get => (string)GetValue(PrimaryTextProperty);
    }

    public static readonly BindableProperty SecondaryTextProperty =
        BindableProperty.Create(nameof(SecondaryText), typeof(string),
            typeof(FancyControl),
            default(string),
            propertyChanged:
            (bindable, oldValue, newValue) =>
                ((FancyControl)bindable).updateSecondaryLabelText());

    public string SecondaryText
    {
        set => SetValue(SecondaryTextProperty, value);
        get => (string)GetValue(SecondaryTextProperty);
    }

    public FancyControl()
	{
		InitializeComponent();
	}

    private void updatePrimaryLabelText()
    {
        // This is null when FancyControlStyle1 is being applied - Text isn't successfully set by the Style
        if (PrimaryLabel != null)
        {
            PrimaryLabel.Text = PrimaryText;
        }
    }

    private void updateSecondaryLabelText()
    {
        invokeWhenConditionIsMet(() => SecondaryLabel != null, () =>
        {
            if (SecondaryLabel != null)
            {
                SecondaryLabel.Text = SecondaryText;
            }
        });
    }

    /// <summary>
    ///     Loops up to 5 times and checks every 100 ms if the provided condition is true, then invokes the given action.
    /// </summary>
    /// <param name="conditionFunc">The condition Func.</param>
    /// <param name="action">The action to invoke.</param>
    private void invokeWhenConditionIsMet(Func<bool> conditionFunc, Action action)
    {
        _ = Task.Run(async () =>
        {
            var count = 0;
            while (count < 5 && !conditionFunc.Invoke())
            {
                await Task.Delay(100);
                count++;
            }

            Dispatcher?.Dispatch(action);
        });
    }
}