using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace cartoon_worker {
    public class Example_main_control : MonoBehaviour
    {

        [Header("six role")]
        public GameObject[] roles;

        [Header("six role btn")]
        public Button[] role_btns;


        private int index = 0;

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < this.roles.Length; i++)
            {
                this.roles[i].SetActive(false);
            }

            this.roles[this.index].SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        //点击角色的按钮的监听事件
        public void on_role_btn_event(int num)
        {
            for (int i = 0; i < this.roles.Length; i++)
            {
                this.roles[i].SetActive(false);
            }
            this.index = num;
            this.roles[this.index].SetActive(true);

            // role_btns[this.index].colors.normalColor = Color.red;


            //ColorBlock cb = this.role_btns[this.index].colors;


            //cb.normalColor = Color.red;
            //////cb.highlightedColor = Color.green;
            //////cb.pressedColor = Color.blue;
            //////cb.disabledColor = Color.black;

            //this.role_btns[this.index].colors = cb;

        }

        //点击动画按钮的监听事件
        public void on_animation_btn_event(int num)
        {

            Animator animator = this.roles[index].GetComponent<Animator>();

            //print(animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                animator.SetTrigger("animation_" + num);
            }
            //animator.GetCurrentAnimationClipState(0)[0].clip.name;
        }
    }
}
