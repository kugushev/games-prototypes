using System;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class FireOrbController : MonoBehaviour
    {
        [SerializeField] private HandMoveConvolution handMoveConvolution;
        [SerializeField] private GameObject vfx;

        private void Awake()
        {
            handMoveConvolution.MoveStart += HandMoveConvolutionOnMoveStart;
            handMoveConvolution.MoveFinished += HandMoveConvolutionOnMoveFinished;
        }

        private void OnDestroy()
        {
            handMoveConvolution.MoveStart -= HandMoveConvolutionOnMoveStart;
            handMoveConvolution.MoveFinished -= HandMoveConvolutionOnMoveFinished;
        }

        private void HandMoveConvolutionOnMoveStart(HandMoveInfo obj)
        {
            vfx.SetActive(true);
        }

        private void HandMoveConvolutionOnMoveFinished(HandMoveInfo startinfo, HandMoveInfo finishinfo)
        {
            vfx.SetActive(false);
        }
    }
}