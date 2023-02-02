using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using RevitAPITrainingLibrary;

namespace CreateFurniture
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<FamilySymbol> FurnitureTypes { get; } = new List<FamilySymbol>(); //тип мебели
        public object Levels { get; } = new List<Level>();  
        public DelegateCommand SaveCommand { get; set; }
        public FamilySymbol SelectedFurnitureType { get; set; }
        public Level SelectedLevel { get; set; }
        public XYZ Point { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            FurnitureTypes = FamilySymbolUtils.GetFamilySymbols(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);                                  

            Point = SelectionUtils.GetPoint(_commandData, "Выберите точку вставки", ObjectSnapTypes.Endpoints);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Point == null || FurnitureTypes == null || SelectedLevel == null)
                return;

            FamilySymbolUtils.CreateFamilyInstance(_commandData, SelectedFurnitureType, Point, SelectedLevel); //вставка мебели

                RaiseCloseRequest();
            
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
