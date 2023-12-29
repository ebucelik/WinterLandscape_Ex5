using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Showcase.Runtime
{
    public class Border : MonoBehaviour
    {

        #region Public Variables

        public float defaultRadius = 5.0f;

        public Snowball snowball;

        public Material borderMaterial;

        #endregion

        #region Private Variables

        private static readonly string PlayerPositionProperty = "_PlayerPosition";
        private static readonly string VisibilityRadiusProperty = "_VisibilityRadius";

        #endregion

        public void Start()
        {

            this.ResetBorderMaterial();
        }

        private void ResetBorderMaterial()
        {
            this.borderMaterial.SetVector(PlayerPositionProperty, Vector4.positiveInfinity);
            this.borderMaterial.SetFloat(VisibilityRadiusProperty, 0.0f);
        }

        private void OnDestroy()
        {
            this.ResetBorderMaterial();
        }

        private void OnDisable()
        {
            this.ResetBorderMaterial();
        }

        void Update()
        {
            float radiusScale = this.snowball.GetRigidbody().transform.localScale.x;

            this.borderMaterial.SetVector(PlayerPositionProperty, this.snowball.GetRigidbody().transform.position);
            this.borderMaterial.SetFloat(VisibilityRadiusProperty, this.defaultRadius * radiusScale);
        }
    }
}
