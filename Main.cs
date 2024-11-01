using MelonLoader;
using MyTrueGear;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq.Expressions;
using System;
using System.ComponentModel.Design;
using static MelonLoader.ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace GORN_TrueGear
{
    public static class BuildInfo
    {
        public const string Name = "GORN_TrueGear"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "TrueGear Mod for GORN"; // Description for the Mod.  (Set as null if none)
        public const string Author = "HuangLY"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class GORN_TrueGear : MelonMod
    {
        private static TrueGearMod _TrueGear = null;
        private static bool canDamage = false;
        public override void OnInitializeMelon()
        {
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(GORN_TrueGear));
            _TrueGear = new TrueGearMod();
            MelonLogger.Msg("OnApplicationStart");
        }

        //*************************************  Arrow  *********************************************

        [HarmonyPrefix, HarmonyPatch(typeof(Bow), "FireArrow")]
        private static void Bow_FireArrow_Prefix(Bow __instance)
        {
            if (__instance.nockedArrow == null)
            {
                return;
            }
            if (__instance.grabbedBy == null || !__instance.grabbedBy.isPlayer)
            {
                return;
            }
            if (__instance.grabbedBy.ownerFist.left)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandShotgunShoot");
                _TrueGear.Play("LeftHandShotgunShoot");
            }
            else
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("RightHandShotgunShoot");
                _TrueGear.Play("RightHandShotgunShoot");
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Bow), "RunSound")]
        private static void Bow_RunSound_Prefix(Bow __instance)
        {
            if (__instance.grabbedBy == null && __instance.nockedArrow == null)
            {
                return;
            }
            if (__instance.grabbedBy == null || !__instance.grabbedBy.isPlayer)
            {
                return;
            }
            if (__instance.grabbedBy.ownerFist.left)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandPickupItem");
                _TrueGear.Play("LeftHandPickupItem");
            }
            else
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("RightHandPickupItem");
                _TrueGear.Play("RightHandPickupItem");
            }
        }

        //**********************************************************************************

        [HarmonyPrefix, HarmonyPatch(typeof(CrossbowCaestus), "Fire")]
        private static void CrossbowCaestus_Fire_Prefix(CrossbowCaestus __instance)
        {
            if (!__instance.ReadyToFire)
            {
                return;
            }
            if (__instance.crankedM < 1f)
            {
                return;
            }
            if (__instance.ownerFist == null)
            {
                return;
            }
            if (__instance.ownerFist.left)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandRifleShoot");
                _TrueGear.Play("LeftHandRifleShoot");
            }
            else
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("RightHandRifleShoot");
                _TrueGear.Play("RightHandRifleShoot");
            }
        }

        //**********************************************************************************

        [HarmonyPostfix, HarmonyPatch(typeof(DamageRelay), "Damage")]
        private static void DamageRelay_Damage_Postfix(DamageRelay __instance, DamageType damageType, GameObject sender, Collision collision, AITargetable responsibleEntity)
        {
            if (damageType == DamageType.Bleed)
            {
                return;
            }
            if (__instance.owner == null || __instance.owner.IsPlayer())
            {
                return;
            }
            if (responsibleEntity == null || !responsibleEntity.IsPlayer() || (GameController.IsPartyMode && (Player)responsibleEntity != GameController.Player))
            {
                return;
            }
            DamagerRigidbody component = sender.GetComponent<DamagerRigidbody>();
            if (component == null)
            {
                return;
            }     
            if (component.isPlayerFist)
            {
                bool isLeft = component.GetComponentInParent<Fist>().left;
                if (isLeft)
                {
                    MelonLogger.Msg("---------------------------------------");
                    MelonLogger.Msg("LeftHandMeleeHit");
                    _TrueGear.Play("LeftHandMeleeHit");
                }
                else
                {
                    MelonLogger.Msg("---------------------------------------");
                    MelonLogger.Msg("RightHandMeleeHit");
                    _TrueGear.Play("RightHandMeleeHit");
                }
                MelonLogger.Msg(component.weaponBase.type);
                return;
            }
            if (component.weaponBase == null || !component.weaponBase.beingWielded || !component.weaponBase.grabbedByHand.isPlayer)
            {
                return;
            }
            if (component.weaponBase.IsTwoHanded)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandMeleeMajorHit");
                MelonLogger.Msg("RightHandMeleeMajorHit");
                _TrueGear.Play("LeftHandMeleeMajorHit");
                _TrueGear.Play("RightHandMeleeMajorHit");
            }
            else if (component.weaponBase.grabbedByHand.ownerFist.left)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandMeleeMajorHit");
                _TrueGear.Play("LeftHandMeleeMajorHit");
            }
            else
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("RightHandMeleeMajorHit");
                _TrueGear.Play("RightHandMeleeMajorHit");
            }
            MelonLogger.Msg(component.weaponBase.type);
        }

        //**********************************************************************************

        //[HarmonyPrefix, HarmonyPatch(typeof(DamagerRigidbody), "OnCollisionEnter")]
        //private static void DamagerRigidbody_OnCollisionEnter_Prefix(DamagerRigidbody __instance)
        //{
        //    canDamage = true;
        //}

        //private static void WasCollisionEnter()
        //{
        //    canDamage = false;
        //}

        //[HarmonyTranspiler, HarmonyPatch(typeof(DamagerRigidbody), "OnCollisionEnter")]
        //private static IEnumerable<CodeInstruction> DamagerRigidbody_OnCollisionEnter_Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    List<CodeInstruction> list = new List<CodeInstruction>();
        //    foreach (CodeInstruction codeInstruction in instructions)
        //    {
        //        list.Add(codeInstruction);
        //        //if (codeInstruction.opcode == OpCodes.Callvirt && (MethodInfo)codeInstruction.operand == AccessTools.Method(typeof(DamageRelay), "Damage", null, null))
        //        if (codeInstruction.opcode == OpCodes.Callvirt && (MethodInfo)codeInstruction.operand == typeof(DamageRelay).GetMethod("Damage", BindingFlags.Public))
        //        {
        //            list.AddRange(new CodeInstruction[]
        //            {
        //                new CodeInstruction(OpCodes.Ldarg_0, null),
        //                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GORN_TrueGear), "WasCollisionEnter", null, null))
        //            });
        //        }
        //    }
        //    return list;
        //}

        //[HarmonyPostfix, HarmonyPatch(typeof(DamagerRigidbody), "OnCollisionEnter")]
        //private static void DamagerRigidbody_OnCollisionEnter_Postfix(DamagerRigidbody __instance, Collision collision)
        //{
        //    if (!canDamage)
        //    {
        //        MelonLogger.Msg("---------------------------------------");
        //        MelonLogger.Msg("HitSomething");
        //    }
        //}

        //**********************************************************************************

        private static void GongSoft()
        {
            MelonLogger.Msg("---------------------------------------");
            MelonLogger.Msg("GongSoft");
            _TrueGear.Play("GongSoft");
        }
        private static void GongMedium()
        {
            MelonLogger.Msg("---------------------------------------");
            MelonLogger.Msg("GongMedium");
            _TrueGear.Play("GongMedium");
        }

        private static void GongHard()
        {
            MelonLogger.Msg("---------------------------------------");
            MelonLogger.Msg("GongHard");
            _TrueGear.Play("GongHard");
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Gong), "RingInternal")]
        private static void Gong_RingInternal_Prefix(Gong __instance)
        {
            GongHard();
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Gong), "OnCollisionEnter")]
        private static IEnumerable<CodeInstruction> Gong_OnCollisionEnter_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = new List<CodeInstruction>();
            foreach (CodeInstruction codeInstruction in instructions)
            {
                if (codeInstruction.opcode == OpCodes.Ldstr && codeInstruction.operand != null && !string.IsNullOrEmpty((string)codeInstruction.operand))
                {
                    if (((string)codeInstruction.operand).Equals("GongSoft"))
                    {
                        list.Add(CodeInstruction.Call(Expression.Lambda<Action>(Expression.Call(null, AccessTools.Method(typeof(GORN_TrueGear), "GongSoft", null, null), Array.Empty<Expression>()), Array.Empty<ParameterExpression>())));
                    }
                    else if (((string)codeInstruction.operand).Equals("GongMed"))
                    {
                        list.Add(CodeInstruction.Call(Expression.Lambda<Action>(Expression.Call(null, AccessTools.Method(typeof(GORN_TrueGear), "GongMedium", null, null), Array.Empty<ParameterExpression>()))));
                    }
                    else if (((string)codeInstruction.operand).Equals("GongHard"))
                    {
                        list.Add(CodeInstruction.Call(Expression.Lambda<Action>(Expression.Call(null, AccessTools.Method(typeof(GORN_TrueGear), "GongHard", null, null), Array.Empty<ParameterExpression>()))));
                    }
                }
                list.Add(codeInstruction);
            }
            return list;
        }

        //**********************************************************************************

        [HarmonyPrefix, HarmonyPatch(typeof(Grabbable), "Grabbed")]
        private static void Grabbable_Grabbed_Prefix(GrabHand hand)
        {
            if (hand.isPlayer)
            {
                if (hand.ownerFist.left)
                {
                    MelonLogger.Msg("---------------------------------------");
                    MelonLogger.Msg("LeftHandPickupItem");
                    _TrueGear.Play("LeftHandPickupItem");
                }
                else
                {
                    MelonLogger.Msg("---------------------------------------");
                    MelonLogger.Msg("RightHandPickupItem");
                    _TrueGear.Play("RightHandPickupItem");
                }
            }
        }

        //**********************************************************************************

        [HarmonyPrefix, HarmonyPatch(typeof(Grapple), "FireGrapple")]
        private static void Grapple_FireGrapple_Prefix(Grapple __instance)
        {
            if (__instance.currentGrappleHead != null)
            {
                return;
            }
            if (__instance.grabHand.ownerFist.left)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandMeleeBombHit");
                _TrueGear.Play("LeftHandMeleeBombHit");
            }
            else
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("RightHandMeleeBombHit");
                _TrueGear.Play("RightHandMeleeBombHit");
            }            
        }

        //**********************************************************************************

        [HarmonyPostfix, HarmonyPatch(typeof(Gun), "Fire")]
        private static void Gun_Fire_Postfix(Gun __instance)
        {
            if (!__instance.wieldedByPlayer)
            {
                return;
            }
            if (__instance.grabbedByHand.ownerFist.left)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("LeftHandPistolShoot");
                _TrueGear.Play("LeftHandPistolShoot");
            }
            else
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("RightHandPistolShoot");
                _TrueGear.Play("RightHandPistolShoot");
            }

        }

        //**********************************************************************************

        //internal static KeyValuePair<float,float> ContactToHapticRotation(Vector3 contactPos, Vector3 targetPos, Vector3 targetForward, float targetHeight)
        //{
        //    float num = Angle(contactPos - targetPos, targetForward);
        //    float num2 = (contactPos.y - targetPos.y) / targetHeight;
        //    return new KeyValuePair<float, float>(num, num2);
        //}

        //private static float Angle(Vector3 fwd, Vector3 targetDir)
        //{
        //    Vector3 vector = new Vector3(fwd.x, 0f, fwd.z);
        //    Vector3 vector2 = new Vector3(targetDir.x, 0f, targetDir.z);
        //    float num = Vector3.Angle(vector, vector2);
        //    if (AngleDir(fwd, targetDir, Vector3.up) == -1)
        //    {
        //        num = 360f - num;
        //        if (num > 359.9999f)
        //        {
        //            num -= 360f;
        //        }
        //        return num;
        //    }
        //    return num;
        //}

        //private static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        //{
        //    float num = Vector3.Dot(Vector3.Cross(fwd, targetDir), up);
        //    if ((double)num > 0.0)
        //    {
        //        return 1;
        //    }
        //    if ((double)num < 0.0)
        //    {
        //        return -1;
        //    }
        //    return 0;
        //}

        internal static KeyValuePair<float, float> GetAngle(Vector3 contactPos, Vector3 targetPos, Vector3 targetForward, float targetHeight)
        {
            Vector3 vector = new Vector3(targetForward.x, 0f, targetForward.z);
            Vector3 vector2 = new Vector3(contactPos.x - targetPos.x, 0f, contactPos.z - targetPos.z);
            float angle = Vector3.Angle(vector, vector2);

            if (Vector3.Dot(Vector3.Cross(targetForward, vector2), Vector3.up) < 0)
            {
                angle = 360f - angle;
            }

            float heightRatio = (contactPos.y - targetPos.y) / targetHeight;

            return new KeyValuePair<float, float>(angle % 360, heightRatio);
        }


        private static void HeartBeat()
        {
            MelonLogger.Msg("---------------------------------------");
            MelonLogger.Msg("HeartBeat");
            _TrueGear.Play("HeartBeat");
        }

        private static void Damage(PlayerDamageRelay __instance,DamageType type, Vector3 pos, GameObject sender , Collision collision, AITargetable responsibleEntity, bool isFloor)
        {
            DamagerRigidbody component1 = sender.GetComponent<DamagerRigidbody>();
            if (component1 == null)
            {
                return;
            }
            if (isFloor)
            {
                return;
            }

            CapsuleCollider component = __instance.transform.GetComponent<CapsuleCollider>();

            float height = component.bounds.size.y;
            Vector3 localPosition = component.transform.localPosition;
            component.transform.localPosition = new Vector3(localPosition.x + component.center.x, localPosition.y + component.center.y, localPosition.z + component.center.z);
            Vector3 newposition = component.transform.position;
            Vector3 newforward = component.transform.forward;

            var angle = GetAngle(pos, newposition, newforward, height);


            if (type == DamageType.Arrow)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg($"PlayerBulletDamage,{angle.Key},{angle.Value}");
                _TrueGear.PlayAngle("PlayerBulletDamage",angle.Key,angle.Value);
                return;
            } 
            MelonLogger.Msg("---------------------------------------");
            MelonLogger.Msg($"DefaultDamage,{angle.Key},{angle.Value}");
            _TrueGear.PlayAngle("DefaultDamage", angle.Key, angle.Value);
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PlayerDamageRelay), "Update")]
        private static IEnumerable<CodeInstruction> PlayerDamageRelay_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = new List<CodeInstruction>();
            foreach (CodeInstruction codeInstruction in instructions)
            {
                if (codeInstruction.opcode == OpCodes.Ldstr && codeInstruction.operand != null && !string.IsNullOrEmpty((string)codeInstruction.operand) && ((string)codeInstruction.operand).Equals("HeartBeat"))
                {
                    list.Add(CodeInstruction.Call(Expression.Lambda<Action>(Expression.Call(null, AccessTools.Method(typeof(GORN_TrueGear), "HeartBeat", null, null), Array.Empty<Expression>()), Array.Empty<ParameterExpression>())));
                }
                list.Add(codeInstruction);
            }
            return list;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PlayerDamageRelay), "Damage")]
        private static IEnumerable<CodeInstruction> PlayerDamageRelay_Damage_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool once = true;
            List<CodeInstruction> list = new List<CodeInstruction>();
            foreach (CodeInstruction codeInstruction in instructions)
            {
                list.Add(codeInstruction);
                if (once && codeInstruction.opcode == OpCodes.Call && (MethodInfo)codeInstruction.operand == AccessTools.Method(typeof(BloodController), "CreateImpact", null, null))
                {
                    once = false;
                    list.AddRange(new CodeInstruction[]
                    {
                        new CodeInstruction(OpCodes.Ldarg_0, null),
                        new CodeInstruction(OpCodes.Ldarg_1, null),
                        new CodeInstruction(OpCodes.Ldarg_S, 4),
                        new CodeInstruction(OpCodes.Ldarg_S, 5),
                        new CodeInstruction(OpCodes.Ldarg_S, 7),
                        new CodeInstruction(OpCodes.Ldarg_S, 8),
                        new CodeInstruction(OpCodes.Ldarg_S, 9),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GORN_TrueGear), "Damage", null, null))
                    });
                }
            }
            return list;
        }

        //**********************************************************************************

        //[HarmonyPrefix, HarmonyPatch(typeof(SurpriseBox), "SpawnSurprise")]
        //private static void SurpriseBox_SpawnSurprise_Prefix(SurpriseBox __instance)
        //{
        //    if (__instance.haveSpawned)
        //    {
        //        return;
        //    }
        //    MelonLogger.Msg("---------------------------------------");
        //    MelonLogger.Msg("SurpriseBoxSpawnSurprise");
        //}

        [HarmonyPrefix, HarmonyPatch(typeof(GameController), "PlayerDied")]
        private static void GameController_PlayerDied_Prefix(GameController __instance)
        {
            if (GameController.instance.state != GameController.State.Finished)
            {
                MelonLogger.Msg("---------------------------------------");
                MelonLogger.Msg("PlayerDeath");
                _TrueGear.Play("PlayerDeath");
            }

        }




    }
}