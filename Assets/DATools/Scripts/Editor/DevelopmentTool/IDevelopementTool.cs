namespace DATools
{
    public interface IDevelopementTool
    {
        string ToolName { get; }
        void Awake();
        void OnDestroy();
        void OnEnable();
        void OnDisable();
        void OnGUI();
    }
}