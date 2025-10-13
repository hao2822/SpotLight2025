// using UnityEngine;
// using UnityEngine.UI;
//
// public class UIManager : MonoBehaviour
// {
//     static UIManager _instance;
//     public static UIManager Instance
//     {
//         get
//         {
//             if (_instance == null)
//                 _instance = FindObjectOfType<UIManager>();
//             return _instance;
//         }
//     }
//
//     [Header("UI 引用")]
//     public Image[] skillIcons = new Image[3];   // 1/2/3 图标
//     public Text[]  useTexts = new Text[3];      // 剩余次数文本
//
//     public void SetSkillIcons(List<SkillConfig> cfgs, int highlight)
//     {
//         for (int i = 0; i < 3; i++)
//         {
//             skillIcons[i].sprite = cfgs[i].icon;
//             useTexts[i].text = SkillManager.Instance.defaultUses.ToString();
//         }
//         HighlightSkill(highlight);
//     }
//
//     public void HighlightSkill(int idx)
//     {
//         for (int i = 0; i < 3; i++)
//             skillIcons[i].color = i == idx ? Color.white : Color.gray * 0.7f;
//     }
//
//     public void UpdateUses(int idx, int left)
//     {
//         useTexts[idx].text = left.ToString();
//     }
// }