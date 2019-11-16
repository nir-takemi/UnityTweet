var OpenNewWindowPlugin = 
{
  OpenNewWindow: function(URL) 
  {
    var url = Pointer_stringify(URL);
    window.open(url, '_blank', 'left=' + (screen.width - 600)*0.5 + ', top=150, width=600, height=300');
  }
}
 
mergeInto(LibraryManager.library, OpenNewWindowPlugin);