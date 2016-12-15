# Winforms.Binder
Two-way binding for Winforms!

Example:

First of all, make sure your viewModel implement the `INotifyPropertyChanged` interface and notify of changes in your properties.

> If your viewModel doesn't `INotifyPropertyChanged`, the binding will be control -> viewmodel direction only.

Then you will be able to:

```csharp
myUserControl.BindTextTo(() => myViewModel.SomeViewModelProperty);
```

Which is the same as:


```csharp
myUserControl.Bind(
() => myControl.Text,
() => myViewModel.SomeViewModelProperty);
```

You can indeed bind to any property as long as types coincide.

```csharp
myUserControl.Bind(
() => myControl.Enabled,
() => myViewModel.SomeBoolProperty);
```

If property types don't coincide, you just:

```csharp
myUserControl.Bind(
() => myControl.Enabled,
() => myViewModel.SomeStringProperty,
viewModelToControlConverter: v => control.Enabled // We don't want to modify the original value.
controlToViewModelConverter: v => v ? "Yes" : "No");
```

> **Pro-Tip** Your bindings are async friendly, they automatically check for Winform control properties and applies this trick: https://msdn.microsoft.com/en-us/library/ms171728(v=vs.110).aspx#Anchor_0

For now there is just this basic level of bindings.