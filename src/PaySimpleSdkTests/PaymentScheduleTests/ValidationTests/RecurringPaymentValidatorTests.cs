#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// The most recent version of this license can be found at: http://opensource.org/licenses/MIT
#endregion

using PaySimpleSdk.Helpers;
using PaySimpleSdk.PaymentSchedules;
using PaySimpleSdk.PaymentSchedules.Validation;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace PaySimpleSdkTests.PaymentScheduleTests.ValidationTests
{
    public class RecurringPaymentValidatorTests
    {
        [Fact]
        public void AccountId_Is_Zero_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { AccountId = 0 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AccountId is required"));
        }

        [Fact]
        public void AccountId_Is_One_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { AccountId = 1 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "AccountId is required"));
        }

        // *************************************************************************************************

        [Fact]
        public void PaymentAmount_Is_Zero_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { PaymentAmount = 0.00M };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "PaymentAmount must be greater than 0"));
        }

        [Fact]
        public void PaymentAmount_Is_One_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { PaymentAmount = 0.01M };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "PaymentAmount must be greater than 0"));
        }

        [Fact]
        public void Type_Is_Payment_Plan_PaymentAmount_Is_Zero_Is_Valid()
        {
            // Arrange
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { PaymentAmount = 0M };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "PaymentAmount must be greater than 0"));
        }

        // *************************************************************************************************

        [Fact]
        public void StartDate_Is_Empty_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { StartDate = null };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "StartDate is required"));
        }

        [Fact]
        public void StartDate_In_The_Past_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { StartDate = DateTime.Now.AddDays(-1) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "StartDate must be in the future"));
        }

        [Fact]
        public void StartDate_Now_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { StartDate = DateTime.Now.AddSeconds(1) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "StartDate must be in the future"));
        }

        [Fact]
        public void StartDate_In_The_Future_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { StartDate = DateTime.Now.AddDays(1) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "StartDate must be in the future"));
        }

        // *************************************************************************************************

        [Fact]
        public void EndDate_Is_Empty_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { EndDate = null };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "EndDate must be after StartDate"));
        }

        [Fact]
        public void EndDate_Is_Before_StartDate_GeneratesError()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "EndDate must be after StartDate"));
        }

        [Fact]
        public void EndDate_Is_After_StartDate_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(4) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "EndDate must be after StartDate"));
        }

        // *************************************************************************************************

        [Fact]
        public void InvoiceNumber_Exceeds_50_Characters_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { InvoiceNumber = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "InvoiceNumber cannot exceed 50 characters"));
        }

        [Fact]
        public void InvoiceNumber_Is_Empty_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { InvoiceNumber = "" };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "InvoiceNumber cannot exceed 50 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void OrderId_Exceeds_50_Characters_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { OrderId = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "OrderId cannot exceed 50 characters"));
        }

        [Fact]
        public void OrderId_Is_Empty_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { OrderId = "" };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "OrderId cannot exceed 50 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void FirstPaymentAmount_Is_Null_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentAmount = null };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentAmount must be greater than 0"));
        }

        [Fact]
        public void FirstPaymentAmount_Is_One_Cent_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentAmount = 0.01M };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentAmount must be greater than 0"));
        }

        // *************************************************************************************************

        [Fact]
        public void FirstPaymentDate_In_Past_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentDate = DateTime.Now.AddDays(-1), StartDate = DateTime.Now.AddDays(2) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentDate must be a current or future date and must be a date before StartDate"));
        }

        [Fact]
        public void FirstPaymentDate_Is_Greater_Than_StartDate_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentDate = DateTime.Now.AddDays(3), StartDate = DateTime.Now.AddDays(2) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentDate must be a current or future date and must be a date before StartDate"));
        }

        [Fact]
        public void FirstPaymentDate_Is_Empty_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentDate = null, StartDate = DateTime.Now.AddDays(2) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentDate must be a current or future date and must be a date before StartDate"));
        }

        [Fact]
        public void FirstPaymentDate_Is_Current_Date_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentDate = DateTime.Now, StartDate = DateTime.Now.AddDays(2) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentDate must be a current or future date and must be a date before StartDate"));
        }

        [Fact]
        public void FirstPaymentDate_In_The_Future_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { FirstPaymentDate = DateTime.Now.AddDays(2), StartDate = DateTime.Now.AddDays(3) };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "FirstPaymentDate must be a current or future date and must be a date before StartDate"));
        }

        // *************************************************************************************************

        [Fact]
        public void ExecutionFrequencyType_Is_Weekly_ExecutionFrequencyParameter_Is_0_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.Weekly, ExecutionFrequencyParameter = 0 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_Weekly_ExecutionFrequencyParameter_Is_8_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.Weekly, ExecutionFrequencyParameter = 8 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_BiWeekly_ExecutionFrequencyParameter_Is_0_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.BiWeekly, ExecutionFrequencyParameter = 0 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_BiWeekly_ExecutionFrequencyParameter_Is_8_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.BiWeekly, ExecutionFrequencyParameter = 8 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_Weekly_ExecutionFrequencyParameter_Is_1_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.Weekly, ExecutionFrequencyParameter = 1 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_BiWeekly_ExecutionFrequencyParameter_Is_1_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.BiWeekly, ExecutionFrequencyParameter = 1 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_Weekly_ExecutionFrequencyParameter_Is_5_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.Weekly, ExecutionFrequencyParameter = 5 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_BiWeekly_ExecutionFrequencyParameter_Is_5_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.BiWeekly, ExecutionFrequencyParameter = 5 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_Weekly_ExecutionFrequencyParameter_Is_7_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.Weekly, ExecutionFrequencyParameter = 7 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_BiWeekly_ExecutionFrequencyParameter_Is_7_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.BiWeekly, ExecutionFrequencyParameter = 7 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly"));
        }

        // *************************************************************************************************

        [Fact]
        public void ExecutionFrequencyType_Is_SpecificDayOfMonth_ExecutionFrequencyParameter_Is_0_Generates_Error()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.SpecificDayOfMonth, ExecutionFrequencyParameter = 0 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is SpecificDayOfMonth.  Note: If you want to bill on the 30th or 31st, use the LastDayOfMonth ExecutionFrequencyType"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_SpecificDayOfMonth_ExecutionFrequencyParameter_Is_1_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.SpecificDayOfMonth, ExecutionFrequencyParameter = 1 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is SpecificDayOfMonth.  Note: If you want to bill on the 30th or 31st, use the LastDayOfMonth ExecutionFrequencyType"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_SpecificDayOfMonth_ExecutionFrequencyParameter_Is_15_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.SpecificDayOfMonth, ExecutionFrequencyParameter = 15 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is SpecificDayOfMonth.  Note: If you want to bill on the 30th or 31st, use the LastDayOfMonth ExecutionFrequencyType"));
        }

        [Fact]
        public void ExecutionFrequencyType_Is_SpecificDayOfMonth_ExecutionFrequencyParameter_Is_31_Is_Valid()
        {
            // Arrange
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { ExecutionFrequencyType = ExecutionFrequencyType.SpecificDayOfMonth, ExecutionFrequencyParameter = 31 };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is SpecificDayOfMonth.  Note: If you want to bill on the 30th or 31st, use the LastDayOfMonth ExecutionFrequencyType"));
        }

        // *************************************************************************************************

        [Fact]
        public void Description_Exceeds_2048_Characters_Generates_Error()
        {
            // Arrange
            var description = new StringBuilder();
            for (int i = 0; i < 2049; i++)
                description.Append("k");

            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { Description = description.ToString() };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Description can not exceed 2048 characters"));
        }

        [Fact]
        public void Description_Is_Empty_Is_Valid()
        {
            // Arrange            
            var validator = new RecurringPaymentValidator<RecurringPayment>();
            var recurringPayment = new RecurringPayment { Description = "" };

            // Act
            var result = validator.Validate(recurringPayment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "Description can not exceed 2048 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void TotalDueAmount_Is_0_Generates_Error()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalDueAmount = 0.00M };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "TotalDueAmount must be greater than 0"));
        }

        [Fact]
        public void TotalDueAmount_Is_1_Cent_Is_Valid()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalDueAmount = 0.01M };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "TotalDueAmount must be greater than 0"));
        }

        // *************************************************************************************************

        [Fact]
        public void TotalNumberOfPayments_Is_0_Generates_Error()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalNumberOfPayments = 0 };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "TotalNumberOfPayments must be a integer between 1 and 99"));
        }

        [Fact]
        public void TotalNumberOfPayments_Is_100_Generates_Error()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalNumberOfPayments = 100 };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "TotalNumberOfPayments must be a integer between 1 and 99"));
        }

        [Fact]
        public void TotalNumberOfPayments_Is_1_Generates_Error()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalNumberOfPayments = 1 };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "TotalNumberOfPayments must be a integer between 1 and 99"));
        }

        [Fact]
        public void TotalNumberOfPayments_Is_50_Generates_Error()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalNumberOfPayments = 50 };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "TotalNumberOfPayments must be a integer between 1 and 99"));
        }

        [Fact]
        public void TotalNumberOfPayments_Is_99_Generates_Error()
        {
            // Arrange            
            var validator = new PaymentPlanValidator();
            var paymentPlan = new PaymentPlan { TotalNumberOfPayments = 99 };

            // Act
            var result = validator.Validate(paymentPlan);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "TotalNumberOfPayments must be a integer between 1 and 99"));
        }
    }
}