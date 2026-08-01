using MelonLoader;

[assembly: MelonInfo(typeof(CustomSkillsAPI.Core), "CustomSkillsAPI", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace CustomSkillsAPI
{
    public class Core : MelonMod
    {
        public static event Action SetUpProgressionSlots = () => { };
        public static event Action AddProgressionSlots = () => { };
        public static event Action AddInherits = () => { };

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public override void OnLateInitializeMelon()
        {
            SetUpProgressionSlots.Invoke();
            AddProgressionSlots.Invoke();
            AddInherits.Invoke();
        }
    }
}