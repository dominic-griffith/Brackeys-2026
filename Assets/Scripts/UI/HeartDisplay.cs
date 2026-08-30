using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Health _health;

    [Header("Container")]
    [SerializeField] private RectTransform _heartContainer;

    [Header("Heart Appearance")]
    [SerializeField] private Sprite _heartSprite;

    private readonly List<Image> _heartImages = new();

    private void Start()
    {
        CreateHeartImages();
        UpdateHearts();
    }

    private void CreateHeartImages()
    {
        if (_health == null ||
            _heartContainer == null ||
            _heartSprite == null)
        {
            Debug.LogWarning(
                "Health, Heart Container, or Heart Sprite is not assigned.",
                this
            );

            return;
        }

        Canvas.ForceUpdateCanvases();

        float heartHeight = _heartContainer.rect.height;

        // Account for padding from a Horizontal Layout Group.
        HorizontalLayoutGroup layoutGroup =
            _heartContainer.GetComponent<HorizontalLayoutGroup>();

        if (layoutGroup != null)
        {
            heartHeight -=
                layoutGroup.padding.top +
                layoutGroup.padding.bottom;
        }

        float spriteAspect =
            _heartSprite.rect.width / _heartSprite.rect.height;

        float heartWidth = heartHeight * spriteAspect;

        ConfigureLayout(heartWidth);

        for (int i = 0; i < _health.MaximumHealth; i++)
        {
            GameObject heartObject = new GameObject(
                $"Heart {i + 1}",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(LayoutElement)
            );

            heartObject.transform.SetParent(
                _heartContainer,
                false
            );

            Image heartImage = heartObject.GetComponent<Image>();
            heartImage.sprite = _heartSprite;
            heartImage.preserveAspect = true;
            heartImage.raycastTarget = false;

            LayoutElement heartLayout =
                heartObject.GetComponent<LayoutElement>();

            heartLayout.preferredWidth = heartWidth;
            heartLayout.preferredHeight = heartHeight;

            _heartImages.Add(heartImage);
        }
    }

    private void ConfigureLayout(float heartWidth)
    {
        HorizontalLayoutGroup layoutGroup =
            _heartContainer.GetComponent<HorizontalLayoutGroup>();

        if (layoutGroup == null)
        {
            layoutGroup =
                _heartContainer.gameObject
                    .AddComponent<HorizontalLayoutGroup>();
        }

        layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.padding = new RectOffset(0, 0, 0, 0);

        int heartCount = _health.MaximumHealth;

        if (heartCount > 1)
        {
            float totalHeartWidth = heartWidth * heartCount;
            float availableSpacing =
                _heartContainer.rect.width - totalHeartWidth;

            layoutGroup.spacing = Mathf.Max(
                0f,
                availableSpacing / (heartCount - 1)
            );
        }
        else
        {
            layoutGroup.spacing = 0f;
        }
    }

    public void UpdateHearts()
    {
        if (_health == null)
        {
            return;
        }

        for (int i = 0; i < _heartImages.Count; i++)
        {
            // Hearts beyond the current health disappear.
            // Because the list goes left-to-right, they disappear
            // from the far right first.
            _heartImages[i].gameObject.SetActive(
                i < _health.CurrentHealth
            );
        }
    }
}
