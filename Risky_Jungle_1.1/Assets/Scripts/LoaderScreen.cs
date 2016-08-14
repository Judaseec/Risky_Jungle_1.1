using UnityEngine;



using com.ethereal.display.components;



using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.com.ethereal.display.components;




public class LoaderScreen
{

    private static LoaderScreen instance;
    EthModalWindow modal;
    EthComponentManager gui;

    private LoaderScreen()
    {
    }


    public void setGui(EthComponentManager gui)
    {
        this.gui = gui;
    }

    public static LoaderScreen getInstance(EthComponentManager gui)
    {
        if (LoaderScreen.instance == null)
        {
            LoaderScreen.instance = new LoaderScreen();
        }

        LoaderScreen.instance.setGui(gui);
        return LoaderScreen.instance;
    }

    // Use this for initialization
    public void showLoader()
    {
        if (gui == null) return;
        modal = gui.AddModalWindow("win");
        modal.gui.AddSprite("Load", (670 / 2) - 32, (480 / 2) - 32, "img:screens/general/loader,wSprite:64,hSprite:64");
    }

    public void hideLoader()
    {
        modal.gui.RemoveAllComponents();
        modal.Remove();
    }
}

