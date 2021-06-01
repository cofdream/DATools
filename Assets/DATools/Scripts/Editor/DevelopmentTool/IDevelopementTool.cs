namespace DATools
{
    public interface IDevelopementTool
    {
        string ToolName { get; }
        void Awake();
        void OnEnable();
        void OnGUI();
        void OnDisable();
        void OnDestroy();
    }
}