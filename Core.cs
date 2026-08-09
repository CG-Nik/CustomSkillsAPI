using Alta;
using MelonLoader;
using System.Reflection;

[assembly: MelonInfo(typeof(CustomSkillsAPI.Core), "CustomSkillsAPI", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace CustomSkillsAPI
{
    public class Core : MelonMod
    {
        public static event Action PreSetUpProgressionSlots = () => { };
        public static event Action SetUpProgressionSlots = () => { };
        public static event Action PostSetUpProgressionSlots = () => { };
        public static event Action PostAddProgressionSlots = () => { };
        public static event Action PostAddInherits = () => { };

        public static List<ProgressionSlot> progressionSlots = [];

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public override void OnLateInitializeMelon()
        {
            PreSetUpProgressionSlots.Invoke();

            SetUpProgressionSlots.Invoke();

            PostSetUpProgressionSlots.Invoke();

            foreach (ProgressionSlot progressionSlot in progressionSlots)
            {
                ProfessionSkillTree professionSkillTree = ProfessionSkillTree.GetProfessionTree(progressionSlot.Path);

                Dictionary<uint, ProgressionSlot> slotMap = (Dictionary<uint, ProgressionSlot>)professionSkillTree.GetType().GetField("slotMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(professionSkillTree);
                slotMap[progressionSlot.Hash] = progressionSlot;
                professionSkillTree.Slots.Add(progressionSlot);
            }

            PostAddProgressionSlots.Invoke();

            foreach (ProgressionSlot progressionSlot in progressionSlots)
            {
                progressionSlot.AddInherit(ProfessionSkillTree.GetProfessionTree(progressionSlot.Path));
            }

            PostAddInherits.Invoke();
        }

        public static void RegisterProfessionSkill(ProfessionSkill professionSkill)
        {
            ProfessionSkill.CheckItems();
            Dictionary<uint, ProfessionSkill> items = (Dictionary<uint, ProfessionSkill>)typeof(HashedGeneralValue<ProfessionSkill>).GetField("items", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            items.Add(professionSkill.Hash, professionSkill);
        }
    }
}