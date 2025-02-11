namespace Application.DTOs.Common;

public record PaginationDto
{
    private int _currentPage = 1;
    private int _itemsCount = 0;
    private int _pagesCount;
    private int _itemsPerPage = 10;

    public int CurrentPage
    {
        get => _currentPage;
        set 
        {
            if (value < 1) _currentPage = 1;
            else _currentPage = value;
        }
    }
    public int ItemsCount { 
        get => _itemsCount;
        set
        {
            if (value < 0) _itemsCount = 0;
            else _itemsCount = value;
        }
    }

    public int PagesCount { 
        get => _pagesCount; 
        set {
            if (value > 50) _pagesCount = 50;
            else if (value < 1) _pagesCount = 1;
            else _pagesCount = value;
		} 
    }
    public int ItemsPerPage {
        get => _itemsPerPage;
        set {
            if (value < 5)
                _itemsPerPage = 10;
            else _itemsPerPage = value;
            _itemsPerPage = value;
        }
    }
}