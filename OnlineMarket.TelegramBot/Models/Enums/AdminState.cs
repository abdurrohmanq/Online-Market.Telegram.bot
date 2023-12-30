namespace OnlineMarket.TelegramBot.Models.Enums;

public enum AdminState
{
    None,
    AdminPage,
    
    //Products
    ManageProductPage,
    WaitingForProductPrice,
    WaitingForPasswordEnter,
    WaitingForSelectionMenu,
    WaitingForSelectProduct,
    WaitingForGetAllProduct,
    WaitingForUpdatePassword,
    WaitingForProductQuantity,
    WaitingForProductSelection,
    WaitingForProductNameEnter,
    WaitingForUpdateProductName,
    WaitingForUpdateProductDesc,
    WaitingForProductDescription,
    WaitingForUpdateProductPrice,
    WaitingForUpdateProductQuantity,
    WaitingForUpdateProductCategory,
    WaitingForUpdateProductProperty,

    //Categories
    ManageCategoryPage,
    WaitingForCategoryId,
    HandleSelectCategoryForGet,
    WaitingForCategoryNameEnter,
    WaitingForCategorySelection,
    WaitingForUpdateCategoryDesc,
    WaitingForCategoryDescription,
    WaitingCategorySelectForUpdate,
    WaitingForCategoryNameForUpdate,
    WaitingForCategoryInfoSelection,
    WaitingForUpdateCategoryProperty,
    WaitingForCategorySelectionForDelete,

    //Filial
    ManageFilialPage,
    WaitingForGetAllFilial,
    WaitingForFilialSelection,
    WaitingForHandleDeleteFilial,
    WaitingForUpdatingFilialName,
    WaitingForCreatingFilialEnter,
    WaitingForUpdatingFilialEnter,

    //Orders
    ManageOrderPage,
    WaitingForOrderIdForDelete,
    WaitingForEnterUserInfo,
}
