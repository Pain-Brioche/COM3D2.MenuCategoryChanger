using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM3D2_MenuCategoryChanger
{
    public enum ValidCategories
    {
        //Clothes
        acchat,
        headset,
        wear,
        skirt,
        onepiece,
        mizugi,
        bra,
        panz,
        Stkg,
        Shoes,

        //Accessories
        acckami,
        megane,
        accHead,
        accHana,
        glove,
        acckubi,
        acckubiwa,
        accude,
        accheso,
        accashi,
        accsenaka,
        accshippo,
        accxxx,
        accnail,
        acctatoo,
        hokuro,
        lip,

        //Hair
        hairf,
        hairr,
        hairt,
        hairs,
        hairaho
    }

    public enum IgnoredCategories
    {
        body,
        head,
        haircolor,
        skin,
        acctatoo,
        accnai,
        underhair,
        mayu,
        eye,
        eye_hi,
        chikubi,
        chikubi_color,
        set_maidwear,
        set_mywear,
        set_underwear,
        set_body,
        folder_eye,
        folder_mayu,
        folder_underhair,
        folder_skin
    }

    public enum SpecialCategories
    {
        accmimi,
        acckamisub,
        accnip
    }

    public enum CategoryCheckResult
    {
        Valid,
        Ignored,
        Special,
        Unknown
    }
}
