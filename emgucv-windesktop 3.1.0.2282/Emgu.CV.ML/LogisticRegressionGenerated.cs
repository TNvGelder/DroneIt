//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
   public static partial class MlInvoke
   {

     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveLogisticRegressionGetLearningRate(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionSetLearningRate(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveLogisticRegressionGetIterations(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionSetIterations(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern LogisticRegression.RegularizationMethod cveLogisticRegressionGetRegularization(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionSetRegularization(
        IntPtr obj,  
        LogisticRegression.RegularizationMethod val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern LogisticRegression.TrainType cveLogisticRegressionGetTrainMethod(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionSetTrainMethod(
        IntPtr obj,  
        LogisticRegression.TrainType val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveLogisticRegressionGetMiniBatchSize(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionSetMiniBatchSize(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionGetTermCriteria(IntPtr obj, ref MCvTermCriteria val);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveLogisticRegressionSetTermCriteria(IntPtr obj, ref MCvTermCriteria val);
     
   }

   public partial class LogisticRegression
   {

     /// <summary>
     /// Learning rate
     /// </summary>
     public double LearningRate
     {
        get { return MlInvoke.cveLogisticRegressionGetLearningRate(_ptr); } 
        set { MlInvoke.cveLogisticRegressionSetLearningRate(_ptr, value); }
     }
     
     /// <summary>
     /// Number of iterations
     /// </summary>
     public int Iterations
     {
        get { return MlInvoke.cveLogisticRegressionGetIterations(_ptr); } 
        set { MlInvoke.cveLogisticRegressionSetIterations(_ptr, value); }
     }
     
     /// <summary>
     /// Kind of regularization to be applied
     /// </summary>
     public LogisticRegression.RegularizationMethod Regularization
     {
        get { return MlInvoke.cveLogisticRegressionGetRegularization(_ptr); } 
        set { MlInvoke.cveLogisticRegressionSetRegularization(_ptr, value); }
     }
     
     /// <summary>
     /// Kind of training method to be applied
     /// </summary>
     public LogisticRegression.TrainType TrainMethod
     {
        get { return MlInvoke.cveLogisticRegressionGetTrainMethod(_ptr); } 
        set { MlInvoke.cveLogisticRegressionSetTrainMethod(_ptr, value); }
     }
     
     /// <summary>
     /// Specifies the number of training samples taken in each step of Mini-Batch Gradient Descent
     /// </summary>
     public int MiniBatchSize
     {
        get { return MlInvoke.cveLogisticRegressionGetMiniBatchSize(_ptr); } 
        set { MlInvoke.cveLogisticRegressionSetMiniBatchSize(_ptr, value); }
     }
     
     /// <summary>
     /// Termination criteria of the algorithm
     /// </summary>
     public MCvTermCriteria TermCriteria
     {
        get { MCvTermCriteria v = new MCvTermCriteria(); MlInvoke.cveLogisticRegressionGetTermCriteria(_ptr, ref v); return v; } 
        set { MlInvoke.cveLogisticRegressionSetTermCriteria(_ptr, ref value); }
     }
     
   }
}