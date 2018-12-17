using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.Entity;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Linq;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private BooksListCtrl newBooksPanel;
    [SerializeField] private BooksListCtrl overdueBooksPanel;
    [SerializeField] private BooksListCtrl searchBooksPanel;
    [SerializeField] private GameObject connectionHelpPanel;
    [SerializeField] private Authentication authenticationCanvas;
    [SerializeField] private WhatNeedFindPanel whatNeedFindPanel;

    private LibraryClass.ReaderRepository readerRepository;
    private LibraryClass.BookRepository bookRepository;

    private LibraryClass.DataBaseContext dbContext;

    public IEnumerable<LibraryClass.BookAndReader> newBookAndReaders;
    public IEnumerable<LibraryClass.BookAndReader> overdueBookAndReaders;
    public IEnumerable<LibraryClass.Book> searchBooks;
    private int userId = -1;


    public void Loged(int UserId)
    {
        userId = UserId;
        ReConnection();
    }

    public void LogOut()
    {
        authenticationCanvas.gameObject.SetActive(true);
        authenticationCanvas.LogOut();
    }

    public void ReConnection()
    {
        if (LibraryClass.DataBaseContext.CheckConnection())
        {
            connectionHelpPanel.SetActive(false);

            dbContext = LibraryClass.DataBaseContext.ReConnection();
            readerRepository = new LibraryClass.ReaderRepository();
            bookRepository = new LibraryClass.BookRepository();

            newBookAndReaders = GetNewBooks();
            overdueBookAndReaders = GetOverdueBooks();
            searchBooks = GetSearchBooks();

            newBooksPanel.SetLastPageNum(GetNewCount());
            overdueBooksPanel.SetLastPageNum(GetOverdueCount());
            searchBooksPanel.SetLastPageNum(bookRepository.GetSearchCount());

            newBooksPanel.SetBooks();
            overdueBooksPanel.SetBooks();
            searchBooksPanel.SetBooks();
        }
        else
        {
            connectionHelpPanel.SetActive(true);
        }
    }

    public ICollection<LibraryClass.BookAndReader> GetNewBooks(int skip=0,int take=2)
    {
        var reader = readerRepository.Get(userId);
        return reader.BookAndReaders.Where(bAR => bAR.DateEnd.UntilDate > DateTime.Now).Skip(skip).Take(take).ToList();
    }

    public ICollection<LibraryClass.BookAndReader> GetOverdueBooks(int skip = 0, int take = 2)
    {
        var reader = readerRepository.Get(userId);
        return reader.BookAndReaders.Where(bAR => bAR.DateEnd.UntilDate < DateTime.Now).Skip(skip).Take(take).ToList();
    }

    public ICollection<LibraryClass.Book> GetSearchBooks(int skip = 0, int take = 2)
    {
        return bookRepository.GetSomeBooks(skip, take).ToList();
    }

    public int GetNewCount()
    {
        var reader = readerRepository.Get(userId);
        return reader.BookAndReaders.Where(bAR => bAR.DateEnd.UntilDate > DateTime.Now).Count();
    }

    public int GetOverdueCount()
    {
        var reader = readerRepository.Get(userId);
        return reader.BookAndReaders.Where(bAR => bAR.DateEnd.UntilDate < DateTime.Now).Count();
    }

    

    public void ResetNewBooks(int skip,int take)
    {
        newBookAndReaders = GetNewBooks(skip,take);
    }

    public void ResetOverdueBooks(int skip, int take)
    {
        overdueBookAndReaders = GetOverdueBooks(skip, take);
    }

    public void ResetSearchBooks(int skip,int take)
    {

        searchBooks = bookRepository.GetSomeBooksByValues(
            whatNeedFindPanel.GetName(),
            whatNeedFindPanel.GetAuthor(),
            whatNeedFindPanel.GetBookType(),
            whatNeedFindPanel.GetGenre(),
            whatNeedFindPanel.GetRate(),
            skip,
            take);

        searchBooksPanel.SetLastPageNum(bookRepository.GetCountBooksByValues
           (
                whatNeedFindPanel.GetName(),
                whatNeedFindPanel.GetAuthor(),
                whatNeedFindPanel.GetBookType(),
                whatNeedFindPanel.GetGenre(),
                whatNeedFindPanel.GetRate())
            );
    }
    public int GetId() { return userId; }

    
}
