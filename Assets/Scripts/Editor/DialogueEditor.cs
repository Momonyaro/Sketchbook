using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DialogueScriptable))]
    public class DialogueEditor : UnityEditor.Editor
    {
        private DialogueScriptable lastInstance = null;
        
        public override void OnInspectorGUI()
        {
            lastInstance = (DialogueScriptable) target;
            bool lateDirty = false;
            
            EditorGUIUtility.labelWidth = 150;
            
            Color fallback = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.14f, 0.93f, 0.15f);
            if (GUILayout.Button("Mark Dirty", GUILayout.Height(40)))
            {
                lateDirty = true;
            }
            GUI.backgroundColor = fallback;
            
            DrawSpeakerEditor();
            
            EditorGUILayout.Space(25);
            
            DrawComponents();
            
            if (lateDirty)
                EditorUtility.SetDirty(target);
        }

        private void DrawComponents()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            if (lastInstance.components.Count > 0)
                if (InsertComponent(0)) return;
            for (int i = 0; i < lastInstance.components.Count; i++)
            {
                EditorGUILayout.Space(15);
                DialogueComponent current = lastInstance.components[i];
                switch (current.GetComponentType())
                {
                    case ComponentTypes.DIALOGUE_BOX:
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal("HelpBox");
                        current.reveal = EditorGUILayout.Toggle(current.reveal);
                        GUILayout.Label("Dialogue Box");
                        EditorGUILayout.EndHorizontal();
                        if (GUILayout.Button("Remove Component", GUILayout.Width(220)))
                        {
                            lastInstance.components.RemoveAt(i);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                        if (current.reveal)
                            current = DrawDialogueBoxEditor((DialogueBoxComponent) current);
                        EditorGUILayout.EndVertical();
                        break;
                    
                    case ComponentTypes.DESTROY:
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal("HelpBox");
                        current.reveal = EditorGUILayout.Toggle(current.reveal);
                        GUILayout.Label("Destroy");
                        EditorGUILayout.EndHorizontal();
                        if (GUILayout.Button("Remove Component", GUILayout.Width(220)))
                        {
                            lastInstance.components.RemoveAt(i);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                        if (current.reveal)
                            current = DrawDestroyEditor((DestroyComponent) current);
                        EditorGUILayout.EndVertical();
                        break;
                    
                    case ComponentTypes.WAIT:
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal("HelpBox");
                        current.reveal = EditorGUILayout.Toggle(current.reveal);
                        GUILayout.Label("Wait Timer");
                        EditorGUILayout.EndHorizontal();
                        if (GUILayout.Button("Remove Component", GUILayout.Width(220)))
                        {
                            lastInstance.components.RemoveAt(i);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                        if (current.reveal)
                            current = DrawWaitEditor((WaitComponent) current);
                        EditorGUILayout.EndVertical();
                        break;
                    
                    case ComponentTypes.SPAWN_OBJECT:
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal("HelpBox");
                        current.reveal = EditorGUILayout.Toggle(current.reveal);
                        GUILayout.Label("Object Spawner");
                        EditorGUILayout.EndHorizontal();
                        if (GUILayout.Button("Remove Component", GUILayout.Width(220)))
                        {
                            lastInstance.components.RemoveAt(i);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                        if (current.reveal)
                            current = DrawSpawnEditor((SpawnObjectComponent) current);
                        EditorGUILayout.EndVertical();
                        break;
                    
                    default: // Error case, ignore this for now
                        break;
                }
                lastInstance.components[i] = current;
                EditorGUILayout.Space(10);
                if (InsertComponent(i + 1)) return;
            }
            
            if (lastInstance.components.Count == 0)
                if (InsertComponent(0)) return;
            
            EditorGUILayout.EndVertical();
        }

        private SpawnObjectComponent DrawSpawnEditor(SpawnObjectComponent spawnObjectComponent)
        {
            spawnObjectComponent.reference = EditorGUILayout.TextField("Component Reference: ", spawnObjectComponent.reference);
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("This Component will spawn a graphical object (i.e. images) onto a canvas.", MessageType.Info);
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            spawnObjectComponent.objectPrefab =
                (GameObject) EditorGUILayout.ObjectField("Prefab: ", spawnObjectComponent.objectPrefab, typeof(GameObject), false);
            EditorGUIUtility.labelWidth = 80;
            spawnObjectComponent.screenPosition = EditorGUILayout.Vector2Field("Screen Pos: ", spawnObjectComponent.screenPosition);
            EditorGUIUtility.labelWidth = 150;
            EditorGUILayout.EndHorizontal();
            
            
            return spawnObjectComponent;
        }

        private WaitComponent DrawWaitEditor(WaitComponent waitComponent)
        {
            waitComponent.reference = EditorGUILayout.TextField("Component Reference: ", waitComponent.reference);
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("This Component will wait for it's timer to run out before passing the torch to the next component.", MessageType.Info);
            EditorGUILayout.Space(10);
            waitComponent.waitTime = EditorGUILayout.FloatField("Timer Length: ", waitComponent.waitTime);

            return waitComponent;
        }

        private DestroyComponent DrawDestroyEditor(DestroyComponent destroyComponent)
        {
            destroyComponent.reference = EditorGUILayout.TextField("Component Reference: ", destroyComponent.reference);
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("This Component will destroy the currently instanced graphic of the referenced component", MessageType.Info);
            EditorGUILayout.Space(10);
            destroyComponent.refToDestroy = GetComponentReference(destroyComponent.refToDestroy);

            return destroyComponent;
        }

        private DialogueBoxComponent DrawDialogueBoxEditor(DialogueBoxComponent boxComponent)
        {
            boxComponent.reference = EditorGUILayout.TextField("Component Reference: ", boxComponent.reference);
            EditorGUILayout.BeginHorizontal();
            boxComponent.objectPrefab =
                (GameObject) EditorGUILayout.ObjectField("Prefab: ", boxComponent.objectPrefab, typeof(GameObject), false);
            EditorGUIUtility.labelWidth = 80;
            boxComponent.screenPosition = EditorGUILayout.Vector2Field("Screen Pos: ", boxComponent.screenPosition);
            EditorGUIUtility.labelWidth = 150;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical("HelpBox");

            if (boxComponent.dialogueBoxes.Count > 0)
                if (InsertDialogueBox(boxComponent, 0))
                    return boxComponent;
            
            for (int i = 0; i < boxComponent.dialogueBoxes.Count; i++)
            {
                DialogueBoxComponent.DialogueBox current = boxComponent.dialogueBoxes[i];
                
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.BeginHorizontal();
                current.speakerReference = GetSpeaker(current.speakerReference);
                if (GUILayout.Button("Remove Dialogue Box"))
                {
                    boxComponent.dialogueBoxes.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                current.buildTime = EditorGUILayout.Slider("Build Time: ", current.buildTime, 0, 1);
                current.text = EditorGUILayout.TextArea(current.text, GUILayout.MinHeight(50));
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);

                boxComponent.dialogueBoxes[i] = current;
                
                if (InsertDialogueBox(boxComponent, i + 1))
                    break;
            }
            
            if (boxComponent.dialogueBoxes.Count == 0)
                if (InsertDialogueBox(boxComponent, 0))
                    return boxComponent;

            EditorGUILayout.EndVertical();
            return boxComponent;
        }

        private string GetSpeaker(string current)
        {
            int lastSelected = 0;
            string[] currentSpeakers = new string[lastInstance.speakers.Length];
            for (int i = 0; i < currentSpeakers.Length; i++)
            {
                currentSpeakers[i] = lastInstance.speakers[i].speakerReference;
                if (currentSpeakers[i].Equals(current))
                    lastSelected = i;
            }

            return currentSpeakers[EditorGUILayout.Popup("Speaker", lastSelected, currentSpeakers)];
        }
        
        private string GetComponentReference(string current)
        {
            int lastSelected = 0;
            string[] currentComponents = new string[lastInstance.components.Count];
            for (int i = 0; i < currentComponents.Length; i++)
            {
                currentComponents[i] = lastInstance.components[i].reference;
                if (currentComponents[i].Equals(current))
                    lastSelected = i;
            }

            return currentComponents[EditorGUILayout.Popup("Components", lastSelected, currentComponents)];
        }
        
        private bool InsertDialogueBox(DialogueBoxComponent boxComponent, int index) //Return true if a new component is added
        {
            EditorGUILayout.BeginVertical("Box");

            if (GUILayout.Button("Insert Dialogue Line" , GUILayout.Width(150)))
            {
                boxComponent.dialogueBoxes.Insert(index, new DialogueBoxComponent.DialogueBox());
                return true;
            }
            
            EditorGUILayout.EndVertical();
            return false;
        }

        private bool InsertComponent(int index) //Return true if a new component is added
        {
            EditorGUILayout.BeginVertical("HelpBox");
            ComponentTypes type = ComponentTypes.NONE;
            type = (ComponentTypes)EditorGUILayout.EnumPopup("Insert Component: ", type);
            EditorGUILayout.EndVertical();
            
            switch (type)
            {
                case ComponentTypes.DIALOGUE_BOX:
                    DialogueBoxComponent dialogueBoxComponent = new DialogueBoxComponent();
                    dialogueBoxComponent.reference = "_diagBox" + index;
                    lastInstance.components.Insert(index, dialogueBoxComponent);
                    EditorUtility.SetDirty(target);
                    
                    return true;
                
                case ComponentTypes.DESTROY:
                    DestroyComponent destroyComponent = new DestroyComponent();
                    destroyComponent.reference = "_destroyComp" + index;
                    lastInstance.components.Insert(index, destroyComponent);
                    EditorUtility.SetDirty(target);
                    
                    return true;
                
                case ComponentTypes.WAIT:
                    WaitComponent waitComponent = new WaitComponent();
                    waitComponent.reference = "_waitComp" + index;
                    lastInstance.components.Insert(index, waitComponent);
                    EditorUtility.SetDirty(target);
                    return true;
                
                case ComponentTypes.SPAWN_OBJECT:
                    SpawnObjectComponent spawnObject = new SpawnObjectComponent();
                    spawnObject.reference = "_spawnObjComp" + index;
                    lastInstance.components.Insert(index, spawnObject);
                    EditorUtility.SetDirty(target);
                    return true;
                
                default:
                    return false;
            }
        }
        
        private void DrawSpeakerEditor()
        {
            GUILayout.Label("Speakers");
            EditorGUILayout.BeginVertical("HelpBox");

            for (int i = 0; i < lastInstance.speakers.Length; i++)
            {
                Speaker current = lastInstance.speakers[i];
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.BeginHorizontal();
                Texture2D tempPhoto = AssetPreview.GetAssetPreview(current.speakerPhoto);
                GUILayout.Label(tempPhoto, GUILayout.Height(60));
                current.speakerPhoto = (Sprite) EditorGUILayout.ObjectField(current.speakerPhoto, typeof(Sprite), false);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                current.speakerName = EditorGUILayout.TextField("Name: ", current.speakerName);
                current.speakerReference = EditorGUILayout.TextField("Reference: ", current.speakerReference);
                EditorGUILayout.EndHorizontal();
                lastInstance.speakers[i] = current;
                if (GUILayout.Button("Remove Speaker: " + current.speakerName))
                {
                    List<Speaker> temp = lastInstance.speakers.ToList();
                    temp.RemoveAt(i);
                    lastInstance.speakers = temp.ToArray();
                    return;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Speaker", GUILayout.Height(30)))
            {
                List<Speaker> temp = lastInstance.speakers.ToList();
                temp.Add(new Speaker("New Speaker", "_newSpeaker" + (temp.Count + 1), null));
                lastInstance.speakers = temp.ToArray();
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}
