using System;

public static class Events
{
    public static event EventHandler RedButtonClicked;
    
    public static void Emit_RedButtonClicked()
        => RedButtonClicked.Invoke(null, EventArgs.Empty);
}
