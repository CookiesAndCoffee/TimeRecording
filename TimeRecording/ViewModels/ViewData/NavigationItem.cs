namespace TimeRecording.ViewModels.ViewData
{
    /// <summary>
    /// Represents an item in the navigation menu of the application.
    /// </summary>
    public class NavigationItem
    {
        public required string Title { get; set; }
        public required string SelectedIcon { get; set; }
        public required string UnselectedIcon { get; set; }
        public required NavigatableViews View { get; set; }

        override public string ToString()
        {
            return Title;
        }
    }
}
