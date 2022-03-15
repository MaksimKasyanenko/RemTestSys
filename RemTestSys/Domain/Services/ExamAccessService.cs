using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.Models;

namespace RemTestSys.Domain.Services;

public class ExamAccessService : IExamAccessService
{
    public ExamAccessService(AppDbContext dbContext){
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    private readonly AppDbContext dbContext;
    public async Task<List<AccessToExamViewModel>> GetAccessListAsync()
    {
        List<AccessToExamViewModel> accs = new();
        accs.AddRange(await dbContext.AccessesToTestForAll
                                     .Include(acc => acc.Test)
                                     .Select(acc => new AccessToExamViewModel{
                                         Id = acc.Id,
                                         ExamName = acc.Test.Name,
                                         ForAll = true,
                                         ForGroup = false,
                                         ForPerson = false
                                     }).ToListAsync());
        accs.AddRange(await dbContext.AccessesToTestForGroup
                                     .Include(acc => acc.Test)
                                     .Include(acc => acc.Group)
                                     .Select(acc => new AccessToExamViewModel{
                                         Id = acc.Id,
                                         ExamName = acc.Test.Name,
                                         TargetName = acc.Group.Name,
                                         ForAll = false,
                                         ForGroup = true,
                                         ForPerson = false
                                     }).ToListAsync());
        accs.AddRange(await dbContext.AccessesToTestForStudent
                                     .Include(acc => acc.Test)
                                     .Include(acc => acc.Student)
                                     .Select(acc => new AccessToExamViewModel{
                                         Id = acc.Id,
                                         ExamName = acc.Test.Name,
                                         TargetName = acc.Student.FullName,
                                         ForAll = false,
                                         ForGroup = false,
                                         ForPerson = true
                                     }).ToListAsync());
        return accs;
    }
    public async Task OpenCommonAccessAsync(int examId)
    {
        if(await dbContext.Tests.AnyAsync(e => e.Id == examId))
        {
            AccessToTestForAll acc = new AccessToTestForAll();
            acc.TestId = examId;
            dbContext.AccessesToTestForAll.Add(acc);
            await dbContext.SaveChangesAsync();
        }else{
            throw new ArgumentException("Exam with specified Id doesn't exist");
        }
    }
    public async Task OpenGroupAccessAsync(int groupId, int examId)
    {
        if(!await dbContext.Groups.AnyAsync(g => g.Id == groupId))
            throw new ArgumentException("Group with specified Id doesn't exist");
        if(!await dbContext.Tests.AnyAsync(e => e.Id == examId))
            throw new ArgumentException("Exam with specified Id doesn't exist");
        AccessToTestForGroup acc = new AccessToTestForGroup();
        acc.TestId = examId;
        acc.GroupId = groupId;
        dbContext.AccessesToTestForGroup.Add(acc);
        await dbContext.SaveChangesAsync();
    }
    public async Task OpenPersonAccessAsync(int studentId, int examId)
    {
        if(!await dbContext.Students.AnyAsync(s => s.Id == studentId))
            throw new ArgumentException("Student with specified Id doesn't exist");
        if(!await dbContext.Tests.AnyAsync(e => e.Id == examId))
            throw new ArgumentException("Exam with specified Id doesn't exist");
        AccessToTestForStudent acc = new AccessToTestForStudent();
        acc.StudentId = studentId;
        acc.TestId = examId;
        dbContext.AccessesToTestForStudent.Add(acc);
        await dbContext.SaveChangesAsync();
    }
    public async Task CloseAllAccessesAsync()
    {
        dbContext.AccessesToTestForAll.RemoveRange(dbContext.AccessesToTestForAll);
        dbContext.AccessesToTestForGroup.RemoveRange(dbContext.AccessesToTestForGroup);
        dbContext.AccessesToTestForStudent.RemoveRange(dbContext.AccessesToTestForStudent);
        await dbContext.SaveChangesAsync();
    }
    public async Task CloseCommonAccessAsync(int accessId)
    {
        AccessToTestForAll acc = await dbContext.AccessesToTestForAll.SingleAsync(acc => acc.Id == accessId);
        if(acc != null)
        {
            dbContext.AccessesToTestForAll.Remove(acc);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task CloseGroupAccessAsync(int accessId)
    {
        AccessToTestForGroup acc = await dbContext.AccessesToTestForGroup.SingleAsync(acc => acc.Id == accessId);
        if(acc!=null)
        {
            dbContext.AccessesToTestForGroup.Remove(acc);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task ClosePersonAccessAsync(int accessId)
    {
        AccessToTestForStudent acc = await dbContext.AccessesToTestForStudent.SingleAsync(acc => acc.Id == accessId);
        if(acc!=null)
        {
            dbContext.AccessesToTestForStudent.Remove(acc);
            await dbContext.SaveChangesAsync();
        }
    }
}