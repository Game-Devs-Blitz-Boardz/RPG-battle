using UnityEngine;
using System.Collections.Generic;

public enum EquipmentType {
    Weapon,
    Armour,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    
    public EquipmentType equipmentType;

    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength; // 1 point increase damage by 1 and crit. power by 1%
    public int agility; // 1 point increase evasion by 1% and crit. chance by 1 %
    public int intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public int vitality; // 1 point increase health by 3 or 5 points

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower; //default value 150%

    [Header("Defensive Stats")]
    public int health;
    public int armour;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;


    public void Effect(Transform _enemyPosition) {

        foreach (var item in itemEffects) {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers() {
        
        PlayerStats playerStats = PlayerManager.instance.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armour.AddModifier(armour);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }

    public void RemoveModifiers() {

        PlayerStats playerStats = PlayerManager.instance.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armour.RemoveModifier(armour);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);

    }

    public override string GetDescription()
    {

        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit. Chance");
        AddItemDescription(critPower, "Crit. Power");

        AddItemDescription(health, "Health");
        AddItemDescription(armour, "Armour");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resist.");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");

        if (descriptionLength < 5) {

            for (int i = 0; i < 5 - descriptionLength; i++) {
                sb.AppendLine();
                sb.Append("");
            }

        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name) {

        if (_value != 0) {

            if (sb.Length > 0) {
                sb.AppendLine();
            }

            if (_value > 0) {
                sb.Append("+ " + _value + " " + _name);
            }

            descriptionLength ++;

        }

    }   

}
