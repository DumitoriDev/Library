using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BooksListCtrl : MonoBehaviour
{
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] protected BookCell bookCell;
    [SerializeField] protected GameController gameController;
    [SerializeField] private Text pageNumText;
    [SerializeField] private GameObject whatNeedToFindPanel;

    public void WahtSearchShow()
    {
        whatNeedToFindPanel.SetActive(true);
    }

    private void OnDisable()
    {
        if (listType == ListType.SearchBooks)
        {
            whatNeedToFindPanel.SetActive(false);
        }
    }

    protected enum ListType { ActiveBooks, OverdueBooks, SearchBooks }
    [SerializeField] protected ListType listType;

    protected enum SortBy { Name, Author, Date, NameInv, AuthorInv, DateInv, Count, CountInv }
    protected bool NameInv = false, AuthorInv = false, DateInv = false, CountInv = false;

    protected List<FullBookInfo> bookCells = new List<FullBookInfo>();
    protected IEnumerable<LibraryClass.BookAndReader> books;


    public int currentPage = 0;
    public int maxCells = 2;
    private int lastPage = 0;

    public void NextPage()
    {
        if (currentPage < lastPage)
        {
            currentPage++;
            ResetBooks();
        }
    }
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ResetBooks();
        }
    }

    public void FirstPage()
    {
        if (currentPage != 0)
        {
            currentPage = 0;
            ResetBooks();
        }
    }

    public void LastPage()
    {
        if (currentPage != lastPage)
        {
            currentPage = lastPage;
            ResetBooks();
        }
    }

    public void ResetBooks()
    {
        switch (listType)
        {
            case ListType.ActiveBooks:
                gameController.ResetNewBooks(currentPage * maxCells, maxCells);
                break;
            case ListType.OverdueBooks:
                gameController.ResetOverdueBooks(currentPage * maxCells, maxCells);
                break;
            case ListType.SearchBooks:
                gameController.ResetSearchBooks(currentPage * maxCells, maxCells);
                break;
            default:
                break;
        }
        SetBooks();
    }

    public void SetLastPageNum(int booksCount)
    {
        if (booksCount == 0)
        {
            lastPage = 0;
            return;
        }
        lastPage = booksCount / maxCells;
        if (booksCount % maxCells == 0)
        {
            lastPage--;
        }
    }

    public void SetBooks()
    {
        ClearBooks();
        pageNumText.text = (currentPage + 1).ToString();

        switch (listType)
        {
            case ListType.ActiveBooks:
                {
                    books = gameController.newBookAndReaders;
                    GetBookWithReaders();
                }
                break;
            case ListType.OverdueBooks:
                {
                    books = gameController.overdueBookAndReaders;
                    GetBookWithReaders();
                }
                break;
            case ListType.SearchBooks:
                {
                    GetBooks();
                }
                break;
            default:
                break;
        }

    }

    public void GetBooks()
    {
        foreach (LibraryClass.Book book in gameController.searchBooks)
        {
                GameObject newCell = Instantiate(bookCell.gameObject);
                newCell.transform.SetParent(scrollRect.content.transform, false);

                FullBookInfo fullBook = GetFullBook(book);

                bookCells.Add(fullBook);
                newCell.GetComponent<BookCell>().SetValues(fullBook);
        }
    }

    public void GetBookWithReaders()
    {
        foreach (LibraryClass.BookAndReader bookAndReader in books)
        {
            bool skipBook = false;
            int id = gameController.GetId();

            skipBook = false;

            DateTime date = bookAndReader.DateEnd.UntilDate;

            switch (listType)
            {
                case ListType.ActiveBooks:
                    {
                        skipBook = (date < DateTime.Now);
                    }
                    break;
                case ListType.OverdueBooks:
                    {
                        skipBook = (date > DateTime.Now);
                    }
                    break;
                default:
                    break;
            }

            if (skipBook) continue;

            GameObject newCell = Instantiate(bookCell.gameObject);
            newCell.transform.SetParent(scrollRect.content.transform, false);

            FullBookInfo fullBook = GetFullBook(bookAndReader.Book,bookAndReader.DateEnd);
            fullBook.date = date.ToShortDateString();

            bookCells.Add(fullBook);
            newCell.GetComponent<BookCell>().SetValues(fullBook);
        }
    }

    public FullBookInfo GetFullBook(LibraryClass.Book book,LibraryClass.BookDates bookDates=null)
    {
        FullBookInfo fullBook = new FullBookInfo();
        fullBook.bookName = book.Name;
        foreach (LibraryClass.Author writer in book.Author)
        {
            fullBook.author += writer.Name + "\n";
        }
        foreach (LibraryClass.Genre gen in book.Genre)
        {
            fullBook.genre += gen.Name + " ";
        }
        fullBook.pages = book.Pages.ToString();
        fullBook.language = book.Language.Name;
        fullBook.rate = book.Rate.ToString();
        fullBook.desc = book.Desc;
        fullBook.price = book.Price;
        fullBook.type = book.TypeId.Name;
        fullBook.edition = book.Edition.Name;
        fullBook.count = book.Count;
        Texture2D texture = new Texture2D(300, 400);
        texture.LoadImage(book.Cover);
        fullBook.cover = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 100f);

        if (bookDates != null)
        {
            fullBook.startDate = bookDates.StartDate;
            fullBook.untilDate = bookDates.UntilDate;
        }

        return fullBook;
    }



    public void SortByName()
    {
        if (!NameInv) SortCells(SortBy.Name);
        else SortCells(SortBy.NameInv);

        SetSortedCells();
        NameInv = !NameInv;
    }
    public void SortByAuthor()
    {
        if (!AuthorInv) SortCells(SortBy.Author);
        else SortCells(SortBy.AuthorInv);

        SetSortedCells();
        AuthorInv = !AuthorInv;
    }
    public void SortByDate()
    {
        if (!DateInv) SortCells(SortBy.Date);
        else SortCells(SortBy.DateInv);

        SetSortedCells();
        DateInv = !DateInv;
    }

    public void SortByCount()
    {
        if (!CountInv) SortCells(SortBy.Count);
        else SortCells(SortBy.CountInv);

        SetSortedCells();
        CountInv = !CountInv;
    }


    protected void SortCells(SortBy sortType)
    {
        switch (sortType)
        {
            case SortBy.Name:
                bookCells = bookCells.OrderBy(cell => cell.bookName).ToList();
                break;
            case SortBy.Author:
                bookCells = bookCells.OrderBy(cell => cell.author).ToList();
                break;
            case SortBy.Date:
                bookCells = bookCells.OrderBy(cell => cell.date).ToList();
                break;
            case SortBy.Count:
                bookCells = bookCells.OrderBy(cell => cell.count).ToList();
                break;
            case SortBy.NameInv:
                bookCells = bookCells.OrderByDescending(cell => cell.bookName).ToList();
                break;
            case SortBy.AuthorInv:
                bookCells = bookCells.OrderByDescending(cell => cell.author).ToList();
                break;
            case SortBy.DateInv:
                bookCells = bookCells.OrderByDescending(cell => cell.date).ToList();
                break;
            case SortBy.CountInv:
                bookCells = bookCells.OrderByDescending(cell => cell.count).ToList();
                break;
            default:
                break;
        }
    }
    protected void SetSortedCells()
    {
        for (int i = 0; i < bookCells.Count; ++i)
        {
            scrollRect.content.GetChild(i).GetComponent<BookCell>().SetValues(bookCells[i]);
        }
    }
    protected void ClearBooks()
    {
        for (int i = 0; i < scrollRect.content.transform.childCount; ++i)
        {
            Destroy(scrollRect.content.transform.GetChild(i).gameObject);
        }
        bookCells.Clear();
    }
}
