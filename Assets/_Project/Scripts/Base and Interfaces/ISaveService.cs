namespace FirstProject.Shop
{
    public interface ISaveService
    {
        public SaveData Load();

        public void Save(SaveData data);
    }
}
