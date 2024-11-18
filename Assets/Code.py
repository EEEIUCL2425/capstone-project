import ClientSide as api
import random


Products = ["Grocery_CocoCrunch", "Grocery_Danisa", "Grocery_iHop", "Grocery_JollyTime", "Grocery_Mamon", "Grocery_Wafer"]

api.DestroyRoom()
api.BuildPresetStore("Debug", 4)
api.BuildRandomStore(random.randint(0, 100), 10, 10, 4.8, 0.45, 0.4, 0.02, 1, 4, 0.5)
api.FillShelves(26, 270, Products)

