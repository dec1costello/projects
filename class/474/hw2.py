#!/usr/bin/env python
# coding: utf-8


import numpy as np 
import matplotlib.pyplot as plt

import numpy # import again 
import matplotlib.pyplot # import again 

import numpy.linalg 
import numpy.random


import hashlib


def generate_data(Para1, Para2, seed=0):
    """Generate binary random data
    Para1, Para2: dict, {str:float} for each class, 
      keys are mx (center on x axis), my (center on y axis), 
               ux (sigma on x axis), ux (sigma on y axis), 
               y (label for this class)
    seed: int, seed for NUMPy's random number generator. Not Python's random.
    """
    np.random.seed(seed)
    X1 = np.vstack((np.random.normal(Para1['mx'], Para1['ux'], Para1['N']), 
                       np.random.normal(Para1['my'], Para1['uy'], Para1['N'])))
    X2 = np.vstack((np.random.normal(Para2['mx'], Para2['ux'], Para2['N']), np.random.normal(Para2['my'], Para2['uy'], Para2['N'])))
    Y = np.hstack(( Para1['y']*np.ones(Para1['N']), Para2['y']*np.ones(Para2['N'])  ))            
    X = np.hstack((X1, X2)) 
    X = np.transpose(X)
    return X, Y 

def plot_data_hyperplane(X, y, w, filename):
    """
    X: 2-D np array, each row is a sample, not augmented 
    y: 1-D np array, the labels 
    w: 1-by-3 np array, the last element of which is the bias term
	
    Examples
    --------------
	
    >>> X = np.array([[1,2], \
                         [4,5], \
                         [7,8]]) 
    >>> y = np.array([1,-1,1])
    >>> w = [1, 2, -10]
    >>> filename = "test.png"
    >>> plot_data_hyperplane(X, y, w, filename)
    >>> hashlib.md5(open(filename, 'rb').read()).hexdigest()
    '242f39ee1cf8d7874609b98de8585fa0'
    """
	
    # your code here
    X1 = X[y == 1]
    X2 = X[y == -1]
	
	#I do not have to augment x, dim is 2 so I can plot on plane!!
    
    x_ticks = np.array([np.min(X[:,0]), np.max(X[:,0])])
    y_ticks = -1*(x_ticks * w[0] +w[2])/w[1]
    plt.plot(x_ticks, y_ticks)
    
    plt.xlim(np.min(X[0]), np.max(X[0]))
    plt.ylim(np.min(X[1]), np.max(X[1]))

    plt.plot(X1[:,0],X1[:,1],'or')
    plt.plot(X2[:,0],X2[:,1], 'ob')
    plt.savefig(filename)
	
	#uncomment later!
    matplotlib.pyplot.close('all')

def plot_mse(X, y, filename):
    """
    X: 2-D np array, each row is a sample, not augmented 
    y: 1-D np array

    Examples
    -----------------
    >>> X,y = generate_data(\
        {'mx':1,'my':2, 'ux':0.1, 'uy':1, 'y':1, 'N':20}, \
        {'mx':2,'my':4, 'ux':.1, 'uy':1, 'y':-1, 'N':50},\
        seed=10)
    >>> plot_mse(X, y, 'test1.png')
    array([-1.8650779 , -0.03934209,  2.91707992])
    >>> X,y = generate_data(\
    {'mx':1,'my':-2, 'ux':0.1, 'uy':1, 'y':1, 'N':20}, \
    {'mx':-1,'my':4, 'ux':.1, 'uy':1, 'y':-1, 'N':50},\
    seed=10)
    >>> # print (X, y)
    >>> plot_mse(X, y, 'test2.png')
    array([ 0.93061084, -0.01833983,  0.01127093])
    """
    w = np.array([0,0,0]) # just a placeholder

    # your code here
	
	#My Attept!
	#w = x*xT - x*yT
	
	#augment x
    X =np.append(X,  np.ones((X.shape[0],1)), axis=1)

    #x*xT
    compound = np.matmul( np.transpose(X), X)
    #print(compound.shape)#shape is 2,2!


    #inverse of x*xT
    InverserOfTransposedXProduct = np.linalg.inv(compound)
    #print(InverserOfTransposedXProduct.shape)
    #shape is 2, 2!

    #inverse of x*xT multiplied by x
    xtimesxtTinverted = np.matmul( InverserOfTransposedXProduct, np.transpose(X))
    #print(xtimesxtTinverted.shape)
    #shape is 70, 2!

    #solving for yT 
    #one dimension might not have to transpose because 1 dimension

    #solving for x*yT
    #print(y.shape)
    w = np.matmul(xtimesxtTinverted, y)
    #print(w.shape)
    #print(w)

    # Plot after you have w. 
    plot_data_hyperplane(X, y, w, filename)

    return w

def plot_fisher(X, y, filename): 
    """
    X: 2-D np array, each row is a sample, not augmented 
    y: 1-D np array

    Examples
    -----------------
    >>> X,y = generate_data(\
        {'mx':1,'my':2, 'ux':0.1, 'uy':1, 'y':1, 'N':20}, \
        {'mx':2,'my':4, 'ux':.1, 'uy':1, 'y':-1, 'N':50},\
        seed=10)
    >>> plot_fisher(X, y, 'test3.png')
    array([-1.61707972, -0.0341108 ,  2.54419773])
    >>> X,y = generate_data(\
        {'mx':-1.5,'my':2, 'ux':0.1, 'uy':2, 'y':1, 'N':200}, \
        {'mx':2,'my':-4, 'ux':.1, 'uy':1, 'y':-1, 'N':50},\
        seed=1)
    >>> plot_fisher(X, y, 'test4.png')
    array([-1.54593468,  0.00366625,  0.40890079])
    """

    w = np.array([0,0,0]) # just a placeholder
	

    # your code here 
	
	#w = s^-1 (m1-m2)

    #compute X1, and X2
    X1 = X[y == 1]
    ##print(X1.shape)
    X2 = X[y == -1]
    ##print(X2.shape)

    #compute mi
    m1 = np.mean(X1, axis=0)
    ##print(m1.shape)
    m2 = np.mean(X2, axis=1)
    ##print(m2.shape)

    #S i = (xi - mi)(xi-mi)^T

    #dimensions should be number of feature values
    s1 = np.matmul(    np.transpose(np.subtract(X1,m1)), np.subtract(X1,m1))
    #s1 = np.multiply(    np.transpose(np.subtract(X1,m1)),np.subtract(X1,m1) )

	#S i = (xi - mi)(xi-mi)^T

    ##print(s1.shape)
    s2 = np.matmul(    np.transpose(np.subtract(X2,m2)), np.subtract(X2,m2))
    #s2 = np.multiply(  np.transpose(np.subtract(X2,m2)),   np.subtract(X2,m2))
    ##print(s2.shape)

	#s = s1+s2
    S = np.add(s1,s2)
    ##print(S)

    #inverse of S
    S = np.linalg.inv(S)

	#w = s^-1 (m1-m2)
    m=np.subtract(m1,m2)
    w=np.multiply(S,m)
    ##print(w)

    # Plot after you have w. 
    plot_data_hyperplane(X, y, w, filename)
    return w


if __name__ == "__main__":
    import doctest
    doctest.testmod()