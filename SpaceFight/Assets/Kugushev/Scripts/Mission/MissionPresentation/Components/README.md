# Components
The set of reusable components. Each class is responsible for one area: Navigation, Animations, Movement, etc.
Just attach a component to GameObject with any of BasePresentationModel inheritor.
# Requirements
 - All classes int this folder have to inherit from BaseComponent class.
 - The attached BasePresentationModel inheritor
 - Don't specify Awake. Use override of OnAwake instead
 - Be aware of errors in console in Edit mode. You can find a message if model cant be cast to the desired interface.
