# Wpf.NotificationCenter

![Image of an error expanded in the notification center](https://raw.githubusercontent.com/cwinland/Wpf.NotificationCenter/master/Resources/image-3.png)

## Objective

- Create Toast Notifications
- Create Alerts for the notification Center
- Read previous alerts / toasts in one place.
- Unread indicator
- Customizable

## Contents

- [Installation](#installation)
- [Example Images](#example-images)
- [Customization](#customization)
- [API Documentation](./Help/index.html)
- [Release Notes](#release-notes)

## Installation

### Install NuGet Package (Wpf.NotificationCenter)

Choose one:

- .NET CLI
  - ``` dotnet add package Wpf.NotificationCenter --version 1.0.0 ```
- Package Manager
  - ``` NuGet\Install-Package Wpf.NotificationCenter -Version 1.0.0 ```
- Package Reference
  - ``` <PackageReference Include="Wpf.NotificationCenter" Version="1.*" /> ```
- Paket CLI
  - ``` paket add Wpf.NotificationCenter --version 1.0.0 ```
- Script & Interactive
  - ``` #r "nuget: Wpf.NotificationCenter, 1.0.0" ```
- Cake
  - Install Wpf.NotificationCenter as a Cake Addin
    - ``` #addin nuget:?package=Wpf.NotificationCenter&version=1.0.0 ```
  - Install Wpf.NotificationCenter as a Cake Tool
    - ``` #tool nuget:?package=Wpf.NotificationCenter&version=1.0.0 ```

### App.xaml (Add Theme)

- Include theme resource dictionary

  ``` xaml
  <ResourceDictionary Source="pack://application:,,,/Wpf.NotificationCenter;component/Themes/Generic.xaml" />
  ```
  
### App.xaml.cs (update the services collection)

- Add using statement
  
  ``` c#
  using Wpf.NotificationCenter.Extensions;
  ```

- Add the services to the collection:
  
  ``` c#
  services.UseWpfNotificationCenter();
  ```

### MainWindow.xaml (Add Notification Center)

- Add namespace
  
  ``` xaml
  xmlns:notificationCenter="clr-namespace:Wpf.NotificationCenter;assembly=Wpf.NotificationCenter"
  ```

- Add Notification Center with content inside.
  
  ``` xaml
  <notificationCenter:NotificationCenter
        x:Name="NotificationCenter"
        VerticalAlignment="Stretch"
        HorizontalAlignment="Stretch"
        NewAlertColor="GoldenRod"
        AlertMaxWidth="175"
        MaxNotifications="10"
        BorderBrush="Blue"
        IsItemsAscending="False">
        <notificationCenter:NotificationCenter.Header>
            <!-- Header Content -->
            <c:MainNavMenu Grid.Row="0" Navigate="{Binding DataContext.NavigateCommand, RelativeSource={RelativeSource AncestorType={x:Type c:MainWindow}}}"
                                        ViewContext="{Binding DataContext.ViewContext, RelativeSource={RelativeSource AncestorType={x:Type c:MainWindow}}}" />
        </notificationCenter:NotificationCenter.Header>
        <notificationCenter:NotificationCenter.Content>
            <Grid>
                <!-- Main Content here -->
            </Grid>
        </notificationCenter:NotificationCenter.Content>
  </notificationCenter:NotificationCenter> ```

## Example Images

|                          |                          |
:-------------------------:|:-------------------------:
Notification Center resides in the header. | Toast notifications can be shown.
![Image of the notification center residing in the header](https://raw.githubusercontent.com/cwinland/Wpf.NotificationCenter/master/Resources/image-8.png) | ![Image of an error toast notification](https://raw.githubusercontent.com/cwinland/Wpf.NotificationCenter/master/Resources/image-1.png)
Alert center Notification with collapsed message. | Alert center Notification with expanded message.
![Image of an error collapsed in the notification center](https://raw.githubusercontent.com/cwinland/Wpf.NotificationCenter/master/Resources/image-2.png) | ![Image of an error expanded in the notification center](https://raw.githubusercontent.com/cwinland/Wpf.NotificationCenter/master/Resources/image-3.png)

## Customization

### Notification Center Element Properties

| Property                   | Category  | Value Type          | Default       | Description
:----------------------------|:----------|:--------------------|:--------------|:-----------
AlertMaxHeight               | Size      | Double              | 200           | The alert text content maximum height in the alert center.
AlertMaxWidth                | Size      | Double              | Auto          | The alert maximum width property of the notification center popup.
ButtonHorizontalAlignment    | Alignment | HorizontalAlignment | Right         | Indicates the placement of the Alert Center.
ButtonVerticalAlignment      | Alignment | VerticalAlignment   | Top           | Indicates the placement of the Alert Center.
ButtonZIndex                 | Visual    | Integer             | 999           | Indicates the order in which the button is drawn over content.
BorderBrush (Inherited)      | Color     | Brushes             | Transparent   | Used for line colors in notification center and / or headers.
IsItemsAscending             | Sorting   | Boolean             | False         | Indicates the order of alerts in the notification center.
MaxNotifications             | Behavior  | Byte                | 0 (Unlimited) | The upper limit of notifications allowed in the alert center. Oldest are removed when this number is exceeded.
NewAlertColor                | Color     | Brushes             | Goldenrod     | Color of the icon when there is a new alert.
NewAlertIcon                 | Icon      | PackIconKind        | BellAlert     | The icon when there is a new alert.
NotificationSeconds          | Behavior  | Integer             | 5             | Indicates how long the temporary toast message is displayed.
NoAlertIcon                  | Icon      | PackIconKind        | Notifications | The icon when there are no unread alerts.
PopupPlacement               | Visual    | PlacementMode       | Bottom        | Alert Center Popup Positioning.
PopupStaysOpen               | Behavior  | Boolean             | False         | Indicates if the popup should stay open or automatically close when clicking away.
ShowButtonInHeader           | Behavior  | Boolean             | True          | Sets whether the notification center button is visibile. Set to 'False' to hide the button.
ShowButtonInContent          | Behavior  | Boolean             | False         | Sets whether the notification center button is visibile. Set to 'False' to hide the button.

#### Example

[See usage in main window section.](#mainwindowxaml-add-notification-center)

### Detailed Customization Theme File

Most of the look / feel is defined in the generic.xaml file. Primary theme styles are inherited from the theme of the site.

[Click here to view the theme - generic.xaml](https://raw.githubusercontent.com/cwinland/Wpf.NotificationCenter/master/Wpf.NotificationCenter/Themes/Generic.xaml)

## API Documentation

[API Documentation](https://rawcdn.githack.com/cwinland/Wpf.NotificationCenter/de65ab193fb9845d0351cdffe57d6e3758459657/Help/index.html)

## Release Notes

- Initial Release - v1.0.0.
- Add UI fixes, more customization, fix bugs - v1.1.0.
- Add Notification Options, Customization Options - v1.1.1.
- Add more customization and adjust behaviors - v1.1.5.
