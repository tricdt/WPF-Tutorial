#region Copyright (c) 2000-2024 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{                                                                   }
{                                                                   }
{       Copyright (c) 2000-2024 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2024 Developer Express Inc.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using DevExpress.Mvvm.Utils;
namespace DevExpress.Mvvm.DataModel {
	public interface IRepository<TEntity, TPrimaryKey> : IReadOnlyRepository<TEntity> where TEntity : class {
		TEntity Find(TPrimaryKey primaryKey);
		void Add(TEntity entity);
		void Remove(TEntity entity);
		TEntity Create(bool add = true);
		EntityState GetState(TEntity entity);
		void Update(TEntity entity);
		TEntity Reload(TEntity entity);
		Expression<Func<TEntity, TPrimaryKey>> GetPrimaryKeyExpression { get; }
		TPrimaryKey GetPrimaryKey(TEntity entity);
		bool HasPrimaryKey(TEntity entity);
		void SetPrimaryKey(TEntity entity, TPrimaryKey primaryKey);
	}
	public static class RepositoryExtensions {
		public static Expression<Func<TProjection, bool>> GetProjectionPrimaryKeyEqualsExpression<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TPrimaryKey primaryKey) where TEntity : class {
			return ExpressionHelper.GetKeyEqualsExpression<TEntity, TProjection, TPrimaryKey>(repository.GetPrimaryKeyExpression, primaryKey);
		}
		public static TPrimaryKey GetProjectionPrimaryKey<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TProjection projectionEntity) where TEntity : class {
			return GetProjectionValue(projectionEntity,
				(TEntity x) =>
			{
				if(repository.HasPrimaryKey(x)) {
					return repository.GetPrimaryKey(x);
				}
				return default(TPrimaryKey);
			},
				(TProjection x) => GetProjectionKey(repository, x));
		}
		static TPrimaryKey GetProjectionKey<TEntity, TProjection, TPrimaryKey>(IRepository<TEntity, TPrimaryKey> repository, TProjection projection) where TEntity : class {
			var properties = ExpressionHelper.GetKeyProperties(repository.GetPrimaryKeyExpression);
			var projectionType = projection.GetType();
			if(ExpressionHelper.IsTuple(typeof(TPrimaryKey))) {
				var objects = properties.Select(p => projectionType.GetProperty(p.Name).GetValue(projection, null));
				return ExpressionHelper.MakeTuple<TPrimaryKey>(objects.ToArray());
			}
			var property = properties.Single();
			return (TPrimaryKey)projectionType.GetProperty(property.Name).GetValue(projection, null);
		}
		static void SetProjectionKey<TEntity, TProjection, TPrimaryKey>(IRepository<TEntity, TPrimaryKey> repository, TProjection projectionEntity, TPrimaryKey primaryKey) where TEntity : class {
			var properties = ExpressionHelper.GetKeyProperties(repository.GetPrimaryKeyExpression);
			var values = ExpressionHelper.GetKeyPropertyValues(primaryKey);
			if(properties.Count() != values.Count())
				throw new Exception();
			for(int i = 0; i < values.Count(); i++) {
				var projectionProperty = typeof(TProjection).GetProperty(properties[i].Name);
				projectionProperty.SetValue(projectionEntity, values[i], null);
			}
		}
		public static Expression<Func<TProjection, TPrimaryKey>> GetSinglePropertyPrimaryKeyProjectionProperty<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository) where TEntity : class {
			var properties = ExpressionHelper.GetKeyProperties(repository.GetPrimaryKeyExpression);
			var propertyName = properties.Single().Name;
			var parameter = Expression.Parameter(typeof(TProjection));
			return Expression.Lambda<Func<TProjection, TPrimaryKey>>(Expression.Property(parameter, propertyName), parameter);
		}
		public static void VerifyProjection<TEntity, TProjection, TPrimaryKey>(IRepository<TEntity, TPrimaryKey> repository, Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection) where TEntity : class {
			if(typeof(TProjection) != typeof(TEntity) && projection == null)
				throw new ArgumentException("Projection should not be null when its type is different from TEntity.");
			var entityKeyProperties = ExpressionHelper.GetKeyProperties(repository.GetPrimaryKeyExpression);
			var projectionKeyPropertyCount = entityKeyProperties.Count(p =>
			{
				var properties = TypeDescriptor.GetProperties(typeof(TProjection));
				var property = properties[p.Name];
				return property != null;
			});
			if(projectionKeyPropertyCount != entityKeyProperties.Count()) {
				string tprojectionName = typeof(TProjection).Name;
				string message = string.Format("Projection type {0} should have the same primary key as its corresponding entity", tprojectionName);
				throw new ArgumentException(message, tprojectionName);
			}
		}
		public static void SetProjectionPrimaryKey<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TProjection projectionEntity, TPrimaryKey primaryKey) where TEntity : class {
			if(IsProjection<TEntity, TProjection>(projectionEntity)) {
				SetProjectionKey<TEntity, TProjection, TPrimaryKey>(repository, projectionEntity, primaryKey);
			} else {
				repository.SetPrimaryKey(projectionEntity as TEntity, primaryKey);
			}
		}
		public static TEntity FindExistingOrAddNewEntity<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TProjection projectionEntity, Action<TProjection, TEntity> applyProjectionPropertiesToEntity) where TEntity : class {
			bool projection = IsProjection<TEntity, TProjection>(projectionEntity);
			var entity = repository.Find(repository.GetProjectionPrimaryKey(projectionEntity));
			if(entity == null) {
				if(projection) {
					entity = repository.Create();
				} else {
					entity = projectionEntity as TEntity;
					repository.Add(entity);
				}
			}
			if(projection) {
				applyProjectionPropertiesToEntity(projectionEntity, entity);
			}
			return entity;
		}
		public static bool IsDetached<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TProjection projectionEntity) where TEntity : class {
			return GetProjectionValue(projectionEntity,
				(TEntity x) => repository.GetState(x) == EntityState.Detached,
				(TProjection x) => false);
		}
		public static bool ProjectionHasPrimaryKey<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TProjection projectionEntity) where TEntity : class {
			return GetProjectionValue(projectionEntity,
				(TEntity x) => repository.HasPrimaryKey(x),
				(TProjection x) => true);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool ApplyFilterBeforeSelectForFindActualProjectionByKey { get; set; }
		public static TProjection FindActualProjectionByKey<TEntity, TProjection, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection, TPrimaryKey primaryKey) where TEntity : class {
			if(ApplyFilterBeforeSelectForFindActualProjectionByKey && typeof(TEntity) != typeof(TProjection)) {
				try {
					var keyEqualsExpression = GetProjectionPrimaryKeyEqualsExpression<TEntity, TEntity, TPrimaryKey>(repository, primaryKey);
					return repository.GetFilteredEntities(keyEqualsExpression, projection).ToArray().FirstOrDefault();
				} catch(Exception e) {
					if(e.GetType().FullName == "System.Data.Services.Client.DataServiceQueryException")
						return default(TProjection);
					throw;
				}
			} else {
				var primaryKeyEqualsExpression = GetProjectionPrimaryKeyEqualsExpression<TEntity, TProjection, TPrimaryKey>(repository, primaryKey);
				var result = repository.GetFilteredEntities(null, projection).Where(primaryKeyEqualsExpression).Take(1).ToArray().FirstOrDefault(); 
				return GetProjectionValue(result,
					(TEntity x) => x != null ? repository.Reload(x) : null,
					(TProjection x) => x);
			}
		}
		static TProjectionResult GetProjectionValue<TEntity, TProjection, TEntityResult, TProjectionResult>(TProjection value, Func<TEntity, TEntityResult> entityFunc, Func<TProjection, TProjectionResult> projectionFunc) {
			if(typeof(TEntity) != typeof(TProjection) || typeof(TEntityResult) != typeof(TProjectionResult))
				return projectionFunc(value);
			return (TProjectionResult)(object)entityFunc((TEntity)(object)value);
		}
		static bool IsProjection<TEntity, TProjection>(TProjection projection) {
			return !(projection is TEntity);
		}
	}
}
