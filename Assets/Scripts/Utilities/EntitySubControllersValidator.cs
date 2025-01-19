#if UNITY_EDITOR
namespace FoxCultGames.Utilities
{
    using System;
    using Gameplay.Entities;
    using Gameplay.Entities.Settings;
    using Gameplay.Entities.SubControllers;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public static class EntitySubControllersValidator
    {
        static EntitySubControllersValidator()
        {
            ObjectFactory.componentWasAdded += component =>
            {
                if (component is not ISettingsSubController subController)
                    return;

                if (TryAssertComponentPlacementInHierarchy(component, out var itemPresenter))
                    EnsureHavingRequiredBehaviourSettings(subController, itemPresenter);
            };
        }

        private static bool TryAssertComponentPlacementInHierarchy(Component component, out EntityController entityController)
        {
            if (component.transform.parent.TryGetComponent(out entityController))
                return true;
            
            Debug.LogError($"Component {component.GetType().Name} must be placed under an EntityController.");
            return false;
        }

        private static void EnsureHavingRequiredBehaviourSettings(ISettingsSubController settingsItemBehaviour, EntityController entity)
        {
            var serializedSettings = new SerializedObject(entity.EntitySettings);
            var controllerSettingsProperty = serializedSettings.FindProperty(EntitySettings.ControllerSettingsFieldName);
				
            for (var i = 0; i < controllerSettingsProperty.arraySize; i++)
            {
                var behaviourSettings = controllerSettingsProperty.GetArrayElementAtIndex(i).managedReferenceValue;
                if (behaviourSettings.GetType() == settingsItemBehaviour.SettingsType)
                    return;
            }
				
            controllerSettingsProperty.arraySize++;
            controllerSettingsProperty.GetArrayElementAtIndex(controllerSettingsProperty.arraySize - 1).managedReferenceValue = Activator.CreateInstance(settingsItemBehaviour.SettingsType);
			
            serializedSettings.ApplyModifiedProperties();
        }
    }
}
#endif