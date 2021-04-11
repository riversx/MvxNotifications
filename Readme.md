# Xamarin.Forms (iOS+Android) MvvmCross scaffolding with basic notifications management

I started this project to learn:

* How to use MvvmCross scaffolding 
* How to manage notifications 
* How to handle ViewModel <-> View interaction
* How to handle View <-> ViewModel interaction

## Project steps 

### Mvvm scaffolding 

_Apr 10th 2021_

If the MvvmCross templates are not yet installed:
```sh 
dotnet new --install MvxScaffolding.Templates
```

Creation of the new project:
```sh 
dotnet new mvxforms --name MvxNotifications --solution-name MvxNotifications
```

### Project update and improvements

_Apr 10th 2021_

* Move Texts and Command to HomeViewModel
* Update to Xamarin.Forms 5.0.0.2012

### Add NotificationService

_Apr 10th 2021_

Added `INotiricationService`

```cs
public class NotificationInfo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Message { get; set; }
}

public interface INotificationService
{
    // publischer
    void Publish(NotificationInfo notificationInfo, DateTime? notifyTime = null);

    // listener
    event EventHandler<NotificationInfo> OnNotificationReceived;
    event EventHandler<NotificationInfo> OnNotificationOpened;
    void NotificationReceived(NotificationInfo notificationInfo);
    void NotificationOpened(NotificationInfo notificationInfo);
}
```

### Handles VewModel -> View inreraction with events

_Apr 10th 2021_

```cs
public abstract class BaseViewModel : MvxViewModel
{
    public EventHandler<DisplayAlertEventArgs> DisplayAlert;
    public EventHandler<DisplayQuestionEventArgs> DisplayQuestion;
}
```

### Handles VewModel <-> View inreraction with `IMvxInteraction` 

_Apr 11th 2021_

Implemented View <-> ViewModel interaction using `IMvxInteraction<YesNoQuestion>`