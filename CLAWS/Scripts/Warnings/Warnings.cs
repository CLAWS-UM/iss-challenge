using HoloToolkit.Unity;
using UnityEngine;

namespace Warnings
{
  public class TagalongAction : InteractableAction
  {
    private GameObject warning;

    private void Awake()
    {
      warning = Instantiate(warning);
      warning.SetActive(false);

      Billboard billboard = warning.AddComponent<Billboard>();
      warning.AddComponent<SimpleTagalong>();
    }


  }


}
