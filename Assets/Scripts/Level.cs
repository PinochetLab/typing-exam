using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 0)]
    public class Level : ScriptableObject
    {
        [SerializeField] private string russianName;
        [SerializeField] private List<Sprite> sprites;
        
        public List<Word> Words => sprites.Select(x => new Word(x)).ToList();
        
        public string Name => name;
        public string RussianName => russianName;
    }
}