namespace DATools
{
    public interface IDevelopementTool
    {
        string ToolName { get; }
        void Awake();
        void Destroy();
        void Enable();
        void Disable();
        void OnGUI();
    }
}