


namespace AutoUI.Common
{
    public abstract class AutoTestResource
    {
        public string Name;
        public ResourceLoadTypeEnum ResourceLoadType;
        public string Path;

        public abstract void LoadData(Stream entryStream);

        public abstract void StoreData(Stream entryStream);

        public abstract object ToXml();
        
    }
}
