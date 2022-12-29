namespace MultiPurposeProject.DesingPatterns.Observer
{
    public class Editor
    {

        public IPublisher publisher;

        public Editor() { 
            publisher = new Publisher();
        }

        public void OpenFile(string path)
        {
            publisher.Notify("OpenFile", path);
        }
        
        public void SaveFile(string path)
        {
            publisher.Notify("SaveFile", path);
        }

    }
}
